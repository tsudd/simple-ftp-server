using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class Logger
    {
        readonly bool enableLogging;
        readonly string logfilePath;
        readonly object obj = new object();


        public Logger(LoggingOptions loggingOptions)
        {
            enableLogging = loggingOptions.EnableLogging;
            logfilePath = loggingOptions.LogfilePath;
            if (!File.Exists(logfilePath))
            {
                File.Create(logfilePath);
            }
        }

        

        public void MakeRecord(string message)
        {
            if (!enableLogging)
            {
                return;
            }
            lock(obj)
            {
                using (var write = new StreamWriter(logfilePath, true))
                {
                    write.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} {message}.");
                    write.Flush();
                }
            }
        }
    }
}
