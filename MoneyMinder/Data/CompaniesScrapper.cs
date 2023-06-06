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

namespace MoneyMinder.Data
{
    /// <summary>
    /// That class is to get information from https://www.nzx.com/markets/NZSX table and save it in the database
    /// for further usage in StockMarket.razor page.
    /// </summary>
    public class CompaniesScrapper
    {
        //Declare database access.
        private readonly DatabaseContext _db;

        //Constructor with initialize database access.
        public CompaniesScrapper(DatabaseContext db)
        {
            _db = db;
        }

        public async Task DoScrapping()
        {
            //List of strings that will contain information to push it in the database Stock table.
            List<string> companys = new List<string>();

            //Access https://www.nzx.com/markets/NZSX for getting information from the website.
            //Required: HtmlAgilityPack.NetCore
            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"https://www.nzx.com/markets/NZSX");
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);


            //Checking if the database Stock table contains any information.
            //If the database contains any information it will be deleted to upload new information from the website.
            if (_db.Stock.Count() != 0)
            {
                _db.Stock.RemoveRange(_db.Stock);
                _db.SaveChanges();
            }

            //Scraping required information from https://www.nzx.com/markets/NZSX website.
            //The code extends with requirements of what exactly should be scraped from the website.
            var table = doc.DocumentNode.Descendants("td").Where(node => !node.GetAttributeValue("role", "").Contains("row")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Change")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Volume")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Value")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Type")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("GreenBond")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("TradeCount")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("CurrencyCode")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("IndexedCapitalisation")).ToList();


            //Storing all scraped information into a temporary string for easier manipulation.
            string tempStorage = "";
            foreach (var item in table)
            {
                tempStorage += item.InnerText;
            }

            //Splitting information by new lines and storing it into a new temporary array string.
            string[] temp = tempStorage.Split("\n");
            //Removing all empty elements from the temporary array string and saving information into companys List.
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp[i], ".*[a-zA-Z0-9].*") || !string.IsNullOrWhiteSpace(temp[i]))
                {
                    //Removing unwanted characters after '&' sign. For some reason, the scraper gets extra characters after the '&' sign.
                    if (temp[i].Contains('&'))
                    {
                        temp[i] = temp[i].Remove(temp[i].IndexOf('&') + 1, 4);
                    }
                    companys.Add(temp[i].Trim());
                }
            }

            //Storing all information from companys List into the database Stock table.
            for (int n = 0; n < companys.Count; n++)
            {
                var stck = new Stock()
                {
                    StockCode = companys[n],
                    CompanyName = companys[n + 1],
                    MarketPrice = double.Parse(Regex.Replace(companys[n + 2], "[^0-9.]", "")),
                    MarketCap = double.Parse(Regex.Replace(companys[n + 3], "[^0-9.]", ""))
                };

                _db.Stock.Add(stck);
                _db.SaveChanges();

                if (n + 3 >= companys.Count - 3)
                {
                    return;
                }
                else
                {
                    n += 3;
                }
            }
        }
    }
}
