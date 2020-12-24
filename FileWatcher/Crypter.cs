using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    class Crypter
    {
        
        public  byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            else if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            else if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public  string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            else if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            else if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            string plainText;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plainText;
        }

        public async  Task<byte[]> EncryptStringToBytesAsync(string plainText, byte[] key, byte[] IV)
        {
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            else if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            else if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            await swEncrypt.WriteAsync(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        public async  Task<string> DecryptStringFromBytesAsync(byte[] cipherText, byte[] key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            else if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            else if (IV == null || IV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            string plainText;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plainText = await srDecrypt.ReadToEndAsync();
                        }
                    }
                }
            }

            return plainText;
        }
    }
}
