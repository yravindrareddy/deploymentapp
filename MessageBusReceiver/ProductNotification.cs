using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace MessageBusReceiver
{
    public class ProductNotification
    {
        [FunctionName("Notify")]
        public void Run([ServiceBusTrigger("proudctnotification", Connection = "Endpoint=sb://azsrvcbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=eUppXrFJSkAtVtj13TUP7cfxMMFuciWGZ+ASbKL3jnk=")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
