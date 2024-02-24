using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessageBusReceiver
{
    public class ProductNotification
    {
        [FunctionName("Notify1")]
        public async Task Run([ServiceBusTrigger("proudctnotification", Connection = "AzureWebJobsAzureSBConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            using(var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://prod-01.southindia.logic.azure.com:443");
                var stringContent = new StringContent(myQueueItem, System.Text.Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync("/workflows/8d22e4299b334216a1471881269c1dcd/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=peKpizpSHgJ9EfM71tmo-fcgR5qfxXuFSVlv0slZeyI", stringContent);
                string resultContent = await result.Content.ReadAsStringAsync();
                log.LogInformation(resultContent);
            }
        }
    }
}
