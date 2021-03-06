﻿using Model.DataManagerOptions;
using Model.FileManagerOptions;
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
        public ConnectionOptions ConnectionOptions { get; set; } = new ConnectionOptions();
        public SendingOptions SendingOptions { get; set; } = new SendingOptions();

        public string ProccessInfo { get; private set; } = "";

        public ETLOptions()
        {

        }
    }
}
