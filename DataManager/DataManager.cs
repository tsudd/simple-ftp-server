using ConfigManagerService;
using Model.DataManagerOptions;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public class DataManager
    {
        static void Main(string[] args)
        {
            string tablePullFileName = "PersonTablePull_";

            var config = new ConfigManager(@"J:\BSUIRSTUFF\3SEM\Labs\ITP\FileWatcherService\DataManager\config");
            Console.WriteLine("DataManager options loaded.");
            var sendingOptions = config.GetConfiguration<SendingOptions>() as SendingOptions;
            var connectionOptions = config.GetConfiguration<ConnectionOptions>() as ConnectionOptions;
            var transfer = new FileTransferService();
            var xmlGenarator = new XMLGeneratorService(@"J:\BSUIRSTUFF\3SEM\Labs\ITP\FileWatcherService\DataManager\DataPulls");
            Console.WriteLine($"DataManager has been launched with next configurations:\n" +
                $"Sending directory - {sendingOptions.SendingPlace}\n" +
                $"Pull mode - {sendingOptions.PullMode}\n" +
                $"Package size - {sendingOptions.PackageSize}\n\n" +
                $"Data Source - {connectionOptions.DataSource}\n" +
                $"Initial catalog - {connectionOptions.InitialCatalog}\n" +
                $"User - {connectionOptions.User}\n" +
                $"Password - {connectionOptions.Password}\n" +
                $"Integrated Security - {connectionOptions.IntegratedSecurity}");
            var SL = new ServiceLayerService(connectionOptions);
            Console.WriteLine("Press any key to start pulling data.");
            Console.ReadKey();
            Console.WriteLine("\nStart processing...\n");
            if (sendingOptions.PullMode == PullMode.FullTable)
            {
                var persons = SL.GetAllPersons();
                var serilialiezedFilePath = xmlGenarator.GenerateXML(persons, tablePullFileName);
                Console.WriteLine($"Manager had got {1} rows from DB and placed them in {serilialiezedFilePath}");
                _ = transfer.TransferFile(serilialiezedFilePath, sendingOptions.SendingPlace);
                Console.WriteLine($"New file transfered to {sendingOptions.SendingPlace}.");
            } 
            else if (sendingOptions.PullMode == PullMode.ByPackages)
            {
                int currentId = 1;
                int maxId = SL.GetMaxId();
                int packagesAmount = 0;
                while (currentId < maxId)
                {
                    var persons = SL.GetPeopleInRange(currentId, currentId + sendingOptions.PackageSize);
                    var serilialiezedFilePath = xmlGenarator.GenerateXML(persons, tablePullFileName + $"{currentId}-{currentId+sendingOptions.PackageSize}_");
                    _ = transfer.TransferFile(serilialiezedFilePath, sendingOptions.SendingPlace);
                    packagesAmount++;
                    currentId += sendingOptions.PackageSize + 1;
                }
                Console.WriteLine($"Manager had got {packagesAmount} packages from DB and placed them in {sendingOptions.SendingPlace}");
            }
            else if (sendingOptions.PullMode == PullMode.ByJoin)
            {
                var persons = SL.GetPeopleJoin();
                var serilialiezedFilePath = xmlGenarator.GenerateXML(persons, tablePullFileName);
                Console.WriteLine($"Manager had got {1} rows from DB and placed them in {serilialiezedFilePath}");
                _ = transfer.TransferFile(serilialiezedFilePath, sendingOptions.SendingPlace);
                Console.WriteLine($"New file transfered to {sendingOptions.SendingPlace}.");
            }
            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
