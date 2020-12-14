using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    class FileTransferService: IFileTransferService
    {
        public FileTransferService()
        {
        }

        public string TransferFile(string pathToFile, string place)
        {
            if (!Directory.Exists(place))
            {
                throw new ArgumentException();
            }
            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException();
            }
            string distFileName = place + "\\" + pathToFile.Substring(pathToFile.LastIndexOf('\\'));
            File.Copy(pathToFile, distFileName, true);
            return distFileName;
        }
    }
}
