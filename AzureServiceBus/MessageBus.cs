using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Azure_Service_Bus
{
    public class MessageBus : IMessageBus
    {
        private string ServiceConnectionString = "";
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            
            await using var client = new ServiceBusClient(ServiceConnectionString);

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var json = JsonConvert.SerializeObject(message);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(
                Encoding.UTF8.GetBytes(json))
            {
                CorrelationId = Guid.NewGuid().ToString(),
            };

            await sender.SendMessageAsync(serviceBusMessage);
            await client.DisposeAsync();
        }
    }
}
