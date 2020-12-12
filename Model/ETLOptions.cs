using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ETLOptions : Options 
    {
        public PathOptions PathOptions { get; set; } = new PathOptions();
        public ArchiveOptions ArchiveOptions { get; set; } = new ArchiveOptions();
        public LoggingOptions LoggingOptions { get; set; } = new LoggingOptions();
        public EncryptionOptions EncryptionOptions { get; set; } = new EncryptionOptions();

        public string ProccessInfo { get; private set; } = "";

        public ETLOptions()
        {

        }
    }
}
