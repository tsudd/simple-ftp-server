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

namespace FileWatcherService
{
    public partial class ServiceObject : ServiceBase
    {
        private Logger logger;
        private readonly string srcDir;
        private readonly string targetDir;
        private readonly string logfilePath;
        public ServiceObject()
        {
            InitializeComponent();
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
            targetDir = "J:\\BSUIRSTUFF\\3SEM\\Labs\\ITP\\TargetDirectory\\";
            logfilePath = @"J:\BSUIRSTUFF\3SEM\Labs\ITP\logfile.txt";
            srcDir = @"F:\From the Internet\";
        }

        protected override void OnStart(string[] args)
        {
            logger = new Logger(srcDir, targetDir, logfilePath);
            var loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
