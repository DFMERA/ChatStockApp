using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ChatStockApp.Services
{
    public static class UtilServices
    {
        private const string STOCK_URL= "https://stooq.com/q/l/?s=#SYMBOL#&f=sd2t2ohlcv&h&e=csv";
        public static async Task<decimal> GetStockValue(string symbol)
        {
            WebClient myWebClient = new WebClient();

            string urlString = STOCK_URL.Replace("#SYMBOL#",symbol);
            Uri uriString = new Uri(urlString);
            
            Stream myStream = myWebClient.OpenRead(uriString);

            StreamReader sr = new StreamReader(myStream);
            var resp = await sr.ReadToEndAsync();
            myStream.Close();

            var value = Convert.ToDecimal(resp.Split('\n')[1].Split(',')[6]);
            
            return value;
        }
    }
}
