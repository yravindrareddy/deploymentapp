using Azure.Messaging.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connString = "Endpoint=sb://azsrvcbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=N7mJGz3EKZIvzt7CZ4cRzf31eqazq1HWB+ASbKymWPE=";
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient(connString);
            var sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonSerializer.Serialize(message);
            ServiceBusMessage svcMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage)){
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(svcMessage);
            await client.DisposeAsync();
        }
    }
}
