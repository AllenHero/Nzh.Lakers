using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.MQ
{
    public class MqConfig
    {
        public string Host { get; set; }
        public TimeSpan HeartBeat { get; set; }
        public bool AutomaticRecoveryEnabled { get; set; }
        public TimeSpan NetworkRecoveryInterval { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// 死信队列实体
    /// </summary>
    [RabbitMq("dead-letter-{Queue}", ExchangeName = "dead-letter-{exchange}")]
    public class DeadLetterQueue
    {
        public string Body { get; set; }

        public string Exchange { get; set; }

        public string Queue { get; set; }

        public string RoutingKey { get; set; }

        public int RetryCount { get; set; }

        public string ExceptionMsg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }
}
