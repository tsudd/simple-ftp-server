using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConfigManagerService;
using Model.FileManagerOptions;

namespace FileWatcherService
{
    public partial class ServiceObject : ServiceBase
    {
        private Logger logger;
        public string ConfigDirectory { get; private set; } = @"J:\BSUIRSTUFF\3SEM\Labs\ITP\FileWatcherService\FileWatcher\config";
        public ServiceObject()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;

        }

        protected override void OnStart(string[] args)
        {
            var configManager = new ConfigManager(ConfigDirectory);
            
            logger = new Logger(configManager.GetConfiguration<LoggingOptions>() as LoggingOptions);
            var watcher = new Watcher(logger.MakeRecord, configManager);
            var watcherThread = new Thread(new ThreadStart(watcher.Start));
            watcherThread.Start();
            logger.MakeRecord(configManager.ConfigManagingInfo);
        }

        protected override void OnStop()
        {
        }
    }
}
