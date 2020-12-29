using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nzh.Lakers.Job.Job
{
    [DisallowConcurrentExecution]
    public class MyJob : IJob
    {
        private readonly ILogger<MyJob> _logger;

        public MyJob(ILogger<MyJob> logger)
        {
            _logger = logger;
        }

        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Hello World!");
            return Task.CompletedTask;
        }
    }
}
