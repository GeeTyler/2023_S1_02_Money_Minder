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

namespace MoneyMinder.Pages
{
    public class CompoaniesScrapper
    {
        public async Task<List<string>>DoScrapping()
        {
            List <string> companys = new List<string>();

//            HtmlDocument htmlDoc = new HtmlDocument();
            

            HttpClient hc = new HttpClient();
            HttpResponseMessage result = await hc.GetAsync($"https://www.nzx.com/markets/NZSX");
            Stream stream = await result.Content.ReadAsStreamAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(stream);

            var table = doc.DocumentNode.Descendants("td").Where(node=>!node.GetAttributeValue("role","").Contains("row")).ToList();
            var HeaderName = doc.DocumentNode.SelectNodes("//table[@class='no-style']");

            string tempStorage = "";

            foreach(var item in table)
            {
                tempStorage += (item.InnerText);
            }
            string[] temp = tempStorage.Split("\n");
            for (int i = 0; i < temp.Length; i++)
            {
                if (Regex.IsMatch(temp[i], ".*[a-zA-Z0-9].*"))
                {
                    companys.Add(temp[i].Trim());
                }
            }
            List<string> fixedOrder = new List<string>();
            for(int i = 0; i < companys.Count; i++)
            {
                if ((i + 1) % 7 == 0)
                {
                    i += 6;
                }
                else
                {
                    fixedOrder.Add(companys[i].Trim());
                }
            }

            return fixedOrder;
        }
    }
}
