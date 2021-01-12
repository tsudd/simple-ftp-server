using Model;
using Model.FileManagerOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    interface IElement
    {
        void Move(string place);
        void Copy(string place);
        string Compress(string place, ArchiveOptions archiveOptions);
        string Decompress(string place);
        void Encrypt(object alg);
        void Decrypt(object alg);
        Task<string> CompressAsync(string place, ArchiveOptions archiveOptions);
        Task<string> DecompressAsync(string place);
        Task EncryptAsync(object alg);
        Task DecryptAsync(object alg);

        void Delete();
    }
}
