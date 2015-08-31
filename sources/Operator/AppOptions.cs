using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Operator
{
    public class AppOptions
    {
        [Option]
        public bool AutoLogin { get; set; }

        [Option]
        public string Endpoint { get; set; }

        [Option]
        public string SessionId { get; set; }
    }
}