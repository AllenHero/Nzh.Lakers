using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.MQ
{
    [RabbitMq("SkyChen.QueueName", ExchangeName = "SkyChen.ExchangeName", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
