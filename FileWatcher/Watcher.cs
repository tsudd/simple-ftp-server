﻿using ConfigManagerService;
using FileWatcherService.ProssessedObjects;
using Model;
using Model.FileManagerOptions;
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
    public class Watcher
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
            watcher.Filter = "*.xml";

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

        private async void ElementCreated(object sender, FileSystemEventArgs e)
        {
            MakeRecord($"{e.FullPath} was added.\n");
            await TransferElement(e.FullPath);
        }

        private async Task TransferElement(string pathToFile)
        {
            var aesAlg = Aes.Create();
            var addedFile = new FileElement(pathToFile);
            MakeRecord($"1 was added.\n");
            AwaitFileToClose(addedFile.Info.FullName);
            if (enableEncrypting)
            {
                MakeRecord($"2 was added.\n");
                await addedFile.EncryptAsync(aesAlg);
                MakeRecord($"{pathToFile} was encrypted.\n");
            }

            var compressedFile = new FileElement(await addedFile.CompressAsync(targetDir, archiveOptions));
            MakeRecord($"{pathToFile} was compressed.\n");

            var decompressedFile = new FileElement(await compressedFile.DecompressAsync(targetDir));
            MakeRecord($"{decompressedFile.Info.FullName} was moved.\n");

            if (enableEncrypting)
            {
                await decompressedFile.DecryptAsync(aesAlg);
                MakeRecord($"{decompressedFile.Info.FullName} was decrypted.\n");
                await addedFile.DecryptAsync(aesAlg);
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

        private static void AwaitFileToClose(string path)
        {
            while (true)
            {
                try
                {
                    using (var stream = new FileStream(path, FileMode.Open))
                    {
                        return;
                    }
                }
                catch
                {

                }
            }

        }
    }
}
