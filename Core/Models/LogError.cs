﻿
namespace Core.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LogError
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int Type { get; set; }
    }
}
