using Model;
using Model.FileManagerOptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public  class Compresser
    { 
        public  void Compress(string pathToFile, string compressedName, ArchiveOptions archiveOptions)
        {
            using (var sourceStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = File.Open(compressedName, FileMode.Create))
                {
                    using (var compressionStream = new GZipStream(targetStream, archiveOptions.CompressionLevel))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public  void Decompress(string pathToFile, string decompressedName)
        {
            using (var sourceStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = new FileStream(decompressedName, FileMode.Create))
                {
                    using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }

        public async  Task CompressAsync(string pathToFile, string compressedName, ArchiveOptions archiveOptions)
        {
            using (var sourceStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = File.Open(compressedName, FileMode.Create))
                {
                    using (var compressionStream = new GZipStream(targetStream, archiveOptions.CompressionLevel))
                    {
                        await sourceStream.CopyToAsync(compressionStream);
                    }
                }
            }
        }

        public async  Task DecompressAsync(string pathToFile, string decompressedName)
        {
            using (var sourceStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = new FileStream(decompressedName, FileMode.Create))
                {
                    using (var decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        await decompressionStream.CopyToAsync(targetStream);
                    }
                }
            }
        }
    }
}
