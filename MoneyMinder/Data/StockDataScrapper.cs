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

namespace MoneyMinder.Data
{
    public class StockDataScrapper
    {
        private readonly DatabaseContext _db;
        private readonly IDataAccessService _accessService;

        public StockDataScrapper(DatabaseContext db, IDataAccessService accessService)
        {
            _db = db;
            _accessService = accessService;
        }

        public async Task DoScrapping()
        {
            if (_db.MarketPriceData.Count() != 0)
            {
                _db.MarketPriceData.RemoveRange(_db.MarketPriceData);
                await _db.SaveChangesAsync();
            }

            List<string> Stocks = new List<string>();

            string ChosenStock = _accessService.GetChosenStock();
            string fullUrl = "https://nz.finance.yahoo.com/quote/" + ChosenStock + ".NZ" +
                "/history?period1=946771200&period2=1683072000&interval=1d&filter=history&frequency=1d&includeAdjustedClose=true";

            var hc = new HttpClient();
            var result = await hc.GetStringAsync(fullUrl);

            var doc = new HtmlDocument();
            doc.LoadHtml(result);

            var tableRows = doc.DocumentNode.Descendants("table")
                .FirstOrDefault(node => node.GetAttributeValue("data-test", "") == "historical-prices")
                ?.Descendants("tr")
                .Skip(1)
                .Take(70)
                .ToList();

            foreach (var row in tableRows)
            {
                var rowData = row.Descendants("td").Select(node => node.InnerText.Trim()).ToList();

                DateTime date = Convert.ToDateTime(rowData[0]);
                // check if an entity with the same StockCode and Date already exists in the context
                var existingMarketData = _db.MarketPriceData.FirstOrDefault(x => x.Date == date);


                if (existingMarketData == null)
                {
                    var marketData = new MarketPriceData()
                    {
                        StockCode = ChosenStock,
                        Date = date,
                        Open = rowData[1],          
                        High = rowData[2],            
                        Low = rowData[3],
                        Close = rowData[4],
                        AdjClose = rowData[5],
                        Volume = rowData[6]
                    };

                    _db.MarketPriceData.Add(marketData);
                }
                else
                {
                    existingMarketData.Open = rowData[1];
                    existingMarketData.High = rowData[2];
                    existingMarketData.Low = rowData[3];
                    existingMarketData.Close = rowData[4];
                    existingMarketData.AdjClose = rowData[5];
                    existingMarketData.Volume = rowData[6];

                    _db.Update(existingMarketData);
                }

                _db.SaveChanges();
            }
        }
    }
}
