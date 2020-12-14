using ConfigManagerService;
using Model;
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
    class Watcher
    {
        readonly FileSystemWatcher watcher;
        readonly ArchiveOptions archiveOptions;
        readonly bool enableEncrypting;
        readonly bool enableArchiveDir;
        readonly string targetDir;
        readonly string archivedFilesDir;

        public delegate void DoRecord(string message);
        public DoRecord MakeRecord;

        public bool Enabled { get; private set; }

        public Watcher(DoRecord log, ConfigManager config)
        {
            MakeRecord += log;

            var paths = config.GetConfiguration<PathOptions>() as PathOptions;
            watcher = new FileSystemWatcher(paths.SourceDirectory);
            watcher.Deleted += ElementDeleted;
            watcher.Created += ElementCreated;
            //watcher.Changed += ElementChanged;
            watcher.Renamed += ElementRenamed;
            watcher.Filter = "*.txt";

            targetDir = paths.TargetDirectory;
            enableArchiveDir = paths.EnableArchivation;
            archivedFilesDir = paths.ArchiveDirectory;
            if (!Directory.Exists(archivedFilesDir) && enableArchiveDir)
            {
                try
                {
                    Directory.CreateDirectory(archivedFilesDir);
                }
                catch
                {
                    archivedFilesDir = targetDir + "\\archive";
                    Directory.CreateDirectory(archivedFilesDir);
                    MakeRecord($"Couldn't find directory for archive files, so created this one {archivedFilesDir}.\n");
                }
            }
            enableEncrypting = (config.GetConfiguration<EncryptionOptions>() as EncryptionOptions).EnableEncryption;
            archiveOptions = config.GetConfiguration<ArchiveOptions>() as ArchiveOptions;
        }

        private void ElementDeleted(object sender, FileSystemEventArgs e)
        {
            MakeRecord($"{e.FullPath} was deleted.\n");
        }

        private void ElementCreated(object sender, FileSystemEventArgs e)
        {
            MakeRecord($"{e.FullPath} was added.\n");
            TransferElement(e.FullPath);
        }

        private void TransferElement(string pathToFile)
        {
            var aesAlg = Aes.Create();
            var addedFile = new FileElement(pathToFile);

            if (enableEncrypting)
            {
                addedFile.Encrypt(aesAlg);
                MakeRecord($"{pathToFile} was encrypted.\n");
            }

            var compressedFile = new FileElement(addedFile.Compress(targetDir, archiveOptions));
            MakeRecord($"{pathToFile} was compressed.\n");

            var decompressedFile = new FileElement(compressedFile.Decompress(targetDir));
            MakeRecord($"{decompressedFile.Info.FullName} was moved.\n");

            if (enableEncrypting)
            {
                decompressedFile.Decrypt(aesAlg);
                MakeRecord($"{decompressedFile.Info.FullName} was decrypted.\n");
                addedFile.Decrypt(aesAlg);
            }

            if (enableArchiveDir)
            {
                compressedFile.Move(archivedFilesDir);
            };
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

        //private void ElementChanged(object sender, FileSystemEventArgs e)
        //{
        //    RecordEvent("changed", e.FullPath);
        //}

        private void ElementRenamed(object sender, RenamedEventArgs e)
        {
            MakeRecord($"{e.OldFullPath} was renamed in {e.FullPath}.\n");
        }

        public void Start()
        {
            MakeRecord($"\nService was started with the next configuration:\n" +
                $"Target Directory - {targetDir}\nSource Directory - {watcher.Path}\n" +
                $"Enable archivation dir - {enableArchiveDir}\n" +
                $"Archivation dir - {archivedFilesDir}\nEnable encryption - {enableEncrypting}\n" +
                $"Compression type - {archiveOptions.CompressionLevel}");
            watcher.EnableRaisingEvents = true;
            while (Enabled)
            {
                Thread.Sleep(1000);
            }
        }

        public void Stop()
        {
            watcher.EnableRaisingEvents = false;
            Enabled = false;
            MakeRecord("Service was disabled");
        }

    }
}
