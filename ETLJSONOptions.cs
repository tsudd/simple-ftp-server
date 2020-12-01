﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigProvider;

namespace FileWatcherService
{
    class ETLJSONOptions: ETLOptions
    {
        public ETLJSONOptions(string json): base()
        {
            var options = ConfigProvader.Deserialize<ETLOptions>(json, new JSONParser());
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
