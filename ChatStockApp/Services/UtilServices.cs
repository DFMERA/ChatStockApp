using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChatStockApp.Services
{
    public static class UtilServices
    {
        private const string STOCK_URL= "https://stooq.com/q/l/?s=#SYMBOL#&f=sd2t2ohlcv&h&e=csv";

        private const string AZURE_URL = "https://dfmeramyfirstazurefunction.azurewebsites.net/api/ChatStockBot?code=BXfKFeyLHIackk6fMmLkTkevaqjtW4piUDaWQWcx1RPKuJ7sxB2SwQ==";

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

        public static async Task<string> PostBotStockValue(string messageTxt)
        {
            try
            {

                HttpClient myClient = new HttpClient();

                string urlString = AZURE_URL;
                Uri uriString = new Uri(urlString);
                var jsonBody = "{\"messageTxt\": \"" + messageTxt + "\"}";
                var resp = await myClient.PostAsync(uriString, new StringContent(jsonBody, Encoding.UTF8, "application/json")); ;
                
                var value = resp.Content.ReadAsStringAsync().Result;

                return value;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
