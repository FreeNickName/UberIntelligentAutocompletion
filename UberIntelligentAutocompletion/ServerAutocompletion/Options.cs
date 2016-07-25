using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerAutocompletion
{
    class Options
    {
        [Option('s', "source", HelpText = "Path to Dictionary.")]
        public string SourceWords { get; set; }

        [Option('p', "port", HelpText = "Port.")]
        public int Port { get; set; }
    }
}
}
