using Queue.Model;
using System;
using System.Collections.Generic;

namespace Queue.Services.Server
{
    public class ServiceFreeTime
    {
        public Service Service { get; set; }

        public Schedule Schedule { get; set; }

        public TimeSpan[] TimeIntervals { get; set; }

        public int FreeTimeIntervals { get; set; }

        public List<string> Report { get; set; }
    }
}