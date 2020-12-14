using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService.SupportClasses
{
    static class PathHelper
    {
        private static string GetNewNameIfSame(string pathToFile, string format = "")
        {
            string newPath;
            int i = 1;
            do
            {
                newPath = pathToFile + $"({i})" + format;
                i++;
            } while (File.Exists(newPath));
            return newPath;
        }
        public static string GetNameOfNewFile(string pathToFile, string format = "")
        {
            var temp = pathToFile + format;
            if (!File.Exists(temp))
            {
                return temp;
            }
            return GetNewNameIfSame(pathToFile, format);
        }

        public static string GetNameOfNewDir(string pathToDir)
        {
            var temp = pathToDir;
            if (!Directory.Exists(temp))
            {
                return temp;
            }
            return GetNewNameIfSame(pathToDir);
        }

        public static string GetTypeOfTheFile(string fileName)
        {
            var splittedFileName = fileName.Split('.');
            int len = splittedFileName.Length;
            if (len == 0)
            {
                return "";
            }
            return "." + splittedFileName[len - 1];
        }
    }
}
