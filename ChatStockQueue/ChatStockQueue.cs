using System;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ChatStockQueue
{
    public static class ChatStockQueue
    {
        [FunctionName("ChatStockQueue")]
        public static void Run([QueueTrigger("mystoragequeue", Connection = "QueueConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            HttpClient myClient = new HttpClient();

            string urlString = Environment.GetEnvironmentVariable("StockChatAppUrl");

            string uriString = urlString + myQueueItem;

            var resp = myClient.GetStringAsync(uriString);

            log.LogInformation($"C# Queue trigger GET processed: {resp}");

        }
    }
}
