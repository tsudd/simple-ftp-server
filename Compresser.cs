using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public static class Compresser
    { 
        public static void Compress(string pathToFile, string compressedName)
        {
            using (var sourceStream = new FileStream(pathToFile, FileMode.OpenOrCreate))
            {
                using (var targetStream = File.Open(compressedName, FileMode.Create))
                {
                    using (var compressionStream = new GZipStream(targetStream, CompressionLevel.Optimal))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

        public static void Decompress(string pathToFile, string decompressedName)
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
    }
}
