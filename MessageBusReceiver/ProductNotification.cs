using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessageBusReceiver
{
    public class ProductNotification
    {
        [FunctionName("Notify1")]
        public void Run([ServiceBusTrigger("proudctnotification", Connection = "AzureWebJobsAzureSBConnection")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
