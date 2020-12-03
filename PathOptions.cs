using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcherService
{
    public class PathOptions : Options
    {
        [Path]
        public string SourceDirectory { get; set; } = "C:\\FileWatcherByTsudd\\SourceDirectory";
        [Path]
        public string TargetDirectory { get; set; } = "C:\\FileWatcherByTsudd\\TargetDirectory";
        
        public bool EnableArchivation { get; set; } = true;
        [Path]
        public string ArchiveDirectory { get; set; } = "C:\\FileWatcherByTsudd\\archive";

        public PathOptions()
        {

        }
    }
}
