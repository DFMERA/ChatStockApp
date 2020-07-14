using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ChatStockBotFunction
{
    public static class ChatStockBot
    {
        [FunctionName("ChatStockBot")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger bot for stock values.");

            string messageTxt = req.Query["messageTxt"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            messageTxt = messageTxt ?? data?.messageTxt;

            string stockUrl = Environment.GetEnvironmentVariable("StockUrl");

            var command = messageTxt.Split('=');
            decimal respValue;
            string botMsg = "";

            switch (command[0])
            {
                case "/stock":
                    try
                    {
                        WebClient myWebClient = new WebClient();

                        string urlString = stockUrl.Replace("#SYMBOL#", command[1]);
                        Uri uriString = new Uri(urlString);

                        Stream myStream = myWebClient.OpenRead(uriString);

                        StreamReader sr = new StreamReader(myStream);
                        var resp = await sr.ReadToEndAsync();
                        myStream.Close();

                        respValue = Convert.ToDecimal(resp.Split('\n')[1].Split(',')[6]);

                        botMsg = String.Format("{0} quote is ${1} per share", command[1].ToUpper(), respValue.ToString());
                    }
                    catch
                    {
                        botMsg = "Please enter a valid stock symbol";
                    }
                    finally
                    {

                    }
                    //TO DO: Save a history of the bot requests per user
                    break;
            }

            string responseMessage = string.IsNullOrEmpty(botMsg)
                ? "Please enter a valid stock symbol"
                : botMsg;

            string connectionString = Environment.GetEnvironmentVariable("AzureStorageConnectionString");
            QueueClient queue = new QueueClient(connectionString, "mystoragequeue");
            
            if (null != await queue.CreateIfNotExistsAsync())
            {
                log.LogInformation("C# HTTP trigger message created.");
            }

            byte[] responseMessageByte = System.Text.Encoding.UTF8.GetBytes(responseMessage);
            string responseMessage64 = Convert.ToBase64String(responseMessageByte);
            await queue.SendMessageAsync(responseMessage64);

            return new OkObjectResult(responseMessage);
        }
    }
}
