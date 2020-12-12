using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class LoggingOptions : Options
    {
        public bool EnableLogging { get; set; } = true;
        [Path]
        public string LogfilePath { get; set; } = "C:\\FileWatcherByTsudd\\logfile.txt";

        public LoggingOptions()
        { }
    }
}
