using Model;
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
        void Delete();
    }
}
