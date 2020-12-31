using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.MQ
{
    [RabbitMq("nizeheng.QueueName", ExchangeName = "nizeheng.ExchangeName", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
