using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Service_Bus
{
    internal interface IMessageBus
    {
        Task PublishMessage(object message, string topic_queue_name);
    }
}
