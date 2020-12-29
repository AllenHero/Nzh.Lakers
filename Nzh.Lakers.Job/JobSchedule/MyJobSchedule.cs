
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Job.JobSchedule
{
    public class MyJobSchedule
    {
        public MyJobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType;
            CronExpression = cronExpression;
        }

        public Type JobType { get; }

        public string CronExpression { get; }
    }
}
