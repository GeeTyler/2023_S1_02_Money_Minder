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

namespace MoneyMinder.Pages
{
    public class CompaniesScrapper
    {
        private readonly DatabaseContext _db;

        public CompaniesScrapper(DatabaseContext db)
        {
            _db = db;
        }

        public async Task DoScrapping()
        {
            List <string> companys = new List<string>();

            //            HtmlDocument htmlDoc = new HtmlDocument();


            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"https://www.nzx.com/markets/NZSX");
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            if (_db.Stock.Count() != 0)
            {
                _db.Stock.RemoveRange(_db.Stock);
                _db.SaveChanges();
            }

            var table = doc.DocumentNode.Descendants("td").Where(node=>!node.GetAttributeValue("role","").Contains("row")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Change")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Volume")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Value")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("Type")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("GreenBond")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("TradeCount")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("CurrencyCode")).
                Where(node => !node.GetAttributeValue("data-title", "").Contains("IndexedCapitalisation")).ToList();

            string tempStorage = "";

            foreach(var item in table)
            {
                tempStorage += (item.InnerText);
            }
            string[] temp = tempStorage.Split("\n");

            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp[i], ".*[a-zA-Z0-9].*") || !string.IsNullOrWhiteSpace(temp[i]))
                {
                    companys.Add(temp[i].Trim());              
                }
            }

            for (int n = 0; n < companys.Count; n++)
            {
                var stck = new Stock()
                {
                    StockCode = companys[n],
                    CompanyName = companys[n + 1],
                    MarketPrice = companys[n + 2],
                    CompanyDescription = Convert.ToString(companys.Count),
                    MarketCap = companys[n + 3]
                };

                _db.Stock.Add(stck);
                _db.SaveChanges();
                if (n + 3 >= (companys.Count) - 3)
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
