﻿using ConfigProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class ETLXMLOptions: ETLOptions
    {
        public ETLXMLOptions(string xml): base()
        {
            var options = ConfigProvader.Deserialize<ETLOptions>(xml, new XMLParser());
            ArchiveOptions = options.ArchiveOptions;
            PathOptions.ArchiveDirectory = options.PathOptions.ArchiveDirectory;
            PathOptions.TargetDirectory = options.PathOptions.TargetDirectory;
            PathOptions.EnableArchivation = options.PathOptions.EnableArchivation;
            PathOptions.SourceDirectory = options.PathOptions.SourceDirectory;
            EncryptionOptions = options.EncryptionOptions;
            LoggingOptions = options.LoggingOptions;
        }
    }
}
