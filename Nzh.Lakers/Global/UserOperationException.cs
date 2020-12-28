﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nzh.Lakers.Global
{
    public class UserOperationException : Exception
    {
        public UserOperationException() { }
        public UserOperationException(string message) : base(message) { }
        public UserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
