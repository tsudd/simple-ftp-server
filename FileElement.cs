﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class FileElement: IElement
    {
        const string COMPRESSED_TYPE = ".gz";
        public FileInfo Info { get; private set; }

        public FileElement(string pathToFile)
        {
            Info = new FileInfo(pathToFile);
        }
        public void Move(string placeToMove)
        {
            if (!Info.Exists)
            {
                return;
            }
            string pathToMove = PathHelper.GetNameOfNewFile(
                placeToMove + Info.Name.Substring(0, Info.Name.LastIndexOf('.')),
                Info.Extension
                );
            File.Move(Info.FullName, pathToMove);
            Info.Refresh();
        }

        public void Copy(string placeToCopy)
        {
            if (!Info.Exists)
            {
                return;
            }
            string pathToCopy = PathHelper.GetNameOfNewFile(
                placeToCopy + Info.Name.Substring(0, Info.Name.IndexOf(Info.Extension)),
                Info.Extension
                );
            File.Copy(Info.FullName, pathToCopy);
        }

        public string Compress(string place)
        {
            if (!Info.Exists)
            {
                return null;
            }
            string compressedName = PathHelper.GetNameOfNewFile(
                place + Info.Name,
                COMPRESSED_TYPE);
            Compresser.Compress(Info.FullName, compressedName);
            return compressedName;
        }

        public string Decompress(string place)
        {
            if (!Info.Exists)
            {
                return null;
            }
            string decompressedName = PathHelper.GetNameOfNewFile(
                place + Info.Name.Substring(0, Info.Name.IndexOf(".")),
                PathHelper.GetTypeOfTheFile(Info.Name.Substring(0, Info.Name.IndexOf(Info.Extension)))
                );
            Compresser.Decompress(Info.FullName, decompressedName);
            return decompressedName;
        }

        public void Encrypt(object ciphAlg)
        {
            if (ciphAlg == null)
            {
                throw new NullReferenceException("Cipher algorithm is null.");
            }
            Aes alg;
            if (ciphAlg is Aes aes)
            {
                alg = aes;
            }
            else
            {
                throw new ArgumentException("Wrong cipher algorithm.");
            }
            byte[] array;
            using (var fstream = new StreamReader(Info.FullName))
            {
                array = Crypter.EncryptStringToBytes(fstream.ReadToEnd(), alg.Key, alg.IV);
            }
            using (var targetStream = new FileStream(Info.FullName, FileMode.Truncate))
            {
                targetStream.Write(array, 0, array.Length);
            }
        }

        public void Decrypt(object ciphAlg)
        {
            if (ciphAlg == null)
            {
                throw new NullReferenceException("Cipher algorithm is null.");
            }
            Aes alg;
            if (ciphAlg is Aes aes)
            {
                alg = aes;
            }
            else
            {
                throw new ArgumentException("Wrong cipher algorithm.");
            }
            byte[] array;
            using (var fstream = new FileStream(Info.FullName, FileMode.Open))
            {
                array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
            }
            using (var targetStream = new StreamWriter(new FileStream(Info.FullName, FileMode.Truncate)))
            {
                targetStream.Write(Crypter.DecryptStringFromBytes(array, alg.Key, alg.IV));
            }
        }

        public void Delete()
        {
            if (!Info.Exists)
            {
                return;
            }
            File.Delete(Info.FullName);
        }
    }
}
