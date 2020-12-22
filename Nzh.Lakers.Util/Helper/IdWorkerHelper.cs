using Nzh.Lakers.Util.IdHelper;
using Snowflake.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nzh.Lakers.Util.Helper
{
    public class IdWorkerHelper
    {
        private static IdWorker worker = null;
        private static readonly object locker = new object();
        private static SequentialGuidGenerator _generator = null;

        private IdWorkerHelper()
        {
        }

        static IdWorkerHelper()
        {
            if (worker == null)
            {
                worker = new IdWorker(1, 1);
            }
        }

        private static IdWorker GetInstance()
        {
            if (worker == null)
            {
                lock (locker)
                {
                    if (worker == null)
                    {
                        worker = new IdWorker(1, 1);
                    }
                }
            }
            return worker;
        }

        public static long NewId()
        {
            return worker.NextId();
        }

        public static string GenObjectId()
        {
            return ObjectId.GenerateNewStringId();
        }

        public static Guid GenGuidId()
        {
            return _generator.Create();
        }
    }
}
