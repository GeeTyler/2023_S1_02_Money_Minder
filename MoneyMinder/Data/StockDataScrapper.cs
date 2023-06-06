using System;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using MoneyMinder.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoneyMinder.Data;
using System.Threading;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;

namespace MoneyMinder.Data
{
    /*
        The StockDataScrapper class is responsible for scraping stock data from the yahoo finance website
        and saving it to the database.
    */

    public class StockDataScrapper
    {
        private readonly DatabaseContext _db;
        private readonly IDataAccessService _accessService;

        public StockDataScrapper(DatabaseContext db, IDataAccessService accessService)
        {
            _db = db;
            _accessService = accessService;
        }

        //Performs the scraping operation and saves the data to the database
        public async Task DoScrapping()
        {
            //Remove existing market price data from the database
            if (_db.MarketPriceData.Count() != 0)
            {
                _db.MarketPriceData.RemoveRange(_db.MarketPriceData);
                await _db.SaveChangesAsync();
            }

            //Get the chosen stock from the access service
            string ChosenStock = _accessService.GetChosenStock();

            //Construct the URL for scraping
            string fullUrl = "https://nz.finance.yahoo.com/quote/" + ChosenStock + ".NZ" +
                "/history?p=" + ChosenStock + ".NZ";

            //Send a request to the URL and retrieve the HTML content
            var hc = new HttpClient();
            var result = await hc.GetStringAsync(fullUrl);

            //Load the HTML content into an HtmlDocument object for parsing
            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            //Extract the table rows containing the historical price data
            var tableRows = doc.DocumentNode.Descendants("table")
                .FirstOrDefault(node => node.GetAttributeValue("data-test", "") == "historical-prices")
                ?.Descendants("tr")
                .Skip(1).Where(node => !node.InnerHtml.Contains("Dividend") && node.InnerHtml.Count(c => c == '-') <= 1)
                .ToList();

            bool isEndOfData = false;
            var culture = CultureInfo.InvariantCulture;

            //Process each row of the table
            foreach (var row in tableRows)
            {
                var rowData = row.Descendants("td").Select(node => node.InnerText.Trim()).ToList();

                //Check for end of data markers
                if (rowData.Take(6).Any(val => val.Contains("*Close price adjusted for splits.") ||
                    val.Contains("**Close price adjusted for splits and dividend and/or capital gain distributions.")))
                {
                    isEndOfData = true;
                }

                //Stop processing if end of data is reached
                if (isEndOfData)
                {
                    return;
                }
                else
                {
                    //Convert the date string to DateTime format
                    DateTime date = Convert.ToDateTime(rowData[0]);

                    //Check if market data already exists for the date
                    var existingMarketData = _db.MarketPriceData.FirstOrDefault(x => x.Date == date);

                    //Create a new market data entry or update existing entry
                    if (existingMarketData == null)
                    {
                        var marketData = new MarketPriceData()
                        {
                            StockCode = ChosenStock,
                            Date = date,
                            Open = double.Parse(rowData[1], culture),
                            High = double.Parse(rowData[2], culture),
                            Low = double.Parse(rowData[3], culture),
                            Close = double.Parse(rowData[4], culture),
                            AdjClose = double.Parse(rowData[5], culture),
                            Volume = rowData[6]
                        };

                        _db.MarketPriceData.Add(marketData);
                    }
                    else
                    {
                        existingMarketData.Open = double.Parse(rowData[1], culture);
                        existingMarketData.High = double.Parse(rowData[2], culture);
                        existingMarketData.Low = double.Parse(rowData[3], culture);
                        existingMarketData.Close = double.Parse(rowData[4], culture);
                        existingMarketData.AdjClose = double.Parse(rowData[5], culture);
                        existingMarketData.Volume = rowData[6];

                        _db.Update(existingMarketData);
                    }

                    _db.SaveChanges();
                }
            }
        }
    }
}
