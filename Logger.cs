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
    class Logger
    {
        readonly FileSystemWatcher watcher;
        readonly string logfilePath;
        readonly object obj = new object();
        public string TargetDir { get; private set; }
        public bool Enabled { get; private set; }
        public string ArchivedFilesDir { get; private set; }
        public string EncryptedFilesDir { get; private set; }

        public Logger(string sourceDir, string targetDir, string pathToLog)
        {
            watcher = new FileSystemWatcher(sourceDir);
            watcher.Deleted += ElementDeleted;
            watcher.Created += ElementCreated;
            watcher.Changed += ElementChanged;
            watcher.Renamed += ElementRenamed;
            watcher.Filter = "*.txt";
            logfilePath = pathToLog;
            TargetDir = targetDir;
            ArchivedFilesDir = TargetDir + "archived\\";
            if (!Directory.Exists(ArchivedFilesDir))
            {
                Directory.CreateDirectory(ArchivedFilesDir);
            }
        }

        private void ElementDeleted(object sender, FileSystemEventArgs e)
        {
            RecordEvent("deleted", e.FullPath);
        }

        private void ElementCreated(object sender, FileSystemEventArgs e)
        {
            RecordEvent("created", e.FullPath);
            TransferElement(e.FullPath);
        }

        private void TransferElement(string pathToFile)
        {
            var aesAlg = Aes.Create();
            var addedFile = new FileElement(pathToFile);
            addedFile.Encrypt(aesAlg);
            var compressedFile = new FileElement(addedFile.Compress(TargetDir));
            RecordEvent("compressed", compressedFile.Info.FullName);
            var decompressedFile = new FileElement(compressedFile.Decompress(TargetDir));
            RecordEvent("added", decompressedFile.Info.FullName);
            decompressedFile.Decrypt(aesAlg);
            RecordEvent("decrypted", decompressedFile.Info.FullName);
            addedFile.Decrypt(aesAlg);
            compressedFile.Move(ArchivedFilesDir);
            SortFiles(decompressedFile);
        }

        private void SortFiles(FileElement file)
        {
            var creationDate = file.Info.CreationTime;
            var placeToCopy = file.Info.Directory.FullName;
            placeToCopy += "\\" + creationDate.Year.ToString() + "\\";
            if (!Directory.Exists(placeToCopy))
            {
                Directory.CreateDirectory(placeToCopy);
            }
            placeToCopy += creationDate.Month.ToString() + "\\";
            if (!Directory.Exists(placeToCopy))
            {
                Directory.CreateDirectory(placeToCopy);
            }
            placeToCopy += creationDate.Day.ToString() + "\\";
            if (!Directory.Exists(placeToCopy))
            {
                Directory.CreateDirectory(placeToCopy);
            }
            file.Move(placeToCopy);
        }

        private void ElementChanged(object sender, FileSystemEventArgs e)
        {
            RecordEvent("changed", e.FullPath);
        }

        private void ElementRenamed(object sender, RenamedEventArgs e)
        {
            RecordEvent($"renamed in {e.FullPath}", e.OldFullPath);
        }

        private void RecordEvent(string fileEvent, string pathToFile)
        {
            lock(obj)
            {
                using (var write = new StreamWriter(logfilePath, true))
                {
                    write.WriteLine($"{DateTime.Now:dd/MM/yyyy hh:mm:ss} file {pathToFile} was {fileEvent}.");
                    write.Flush();
                }
            }
        }

        public void Start()
        {
            RecordEvent("launched", "Service");
            watcher.EnableRaisingEvents = true;
            while(Enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            Enabled = false;
            RecordEvent("disabled", "Service");
        }
    }
}
