using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherService
{
    public class ConfigManager
    {
        bool jsonConfigParsed = false;
        bool xmlConfigParsed = false;

        ETLOptions defaultConfig;
        ETLJSONOptions jsonConfig;

        public string ConfigManagingInfo { get; } = "";

        public ConfigManager(string mainPath)
        {
            defaultConfig = new ETLOptions();

            string config;
            try
            {
                using (StreamReader stream = new StreamReader($"{mainPath}\\appsettings.json"))
                {
                    config = stream.ReadToEnd();
                }
                jsonConfig = new ETLJSONOptions(config);
                jsonConfigParsed = true;
                ConfigManagingInfo += "JSON configuration parsed.";
            }
            catch
            {
                ConfigManagingInfo += "Couldn't parse JSON";
            }
        }

        public Options GetConfiguration<T>()
        {
            Options ans = null;

            if (jsonConfigParsed)
            {
                ans = GetExactConfiguration<T>(jsonConfig);
            }
            else if (xmlConfigParsed)
            {
                //xml please
            }
            else
            {
                ans = GetExactConfiguration<T>(defaultConfig);
            }

            return ans;
        }

        private Options GetExactConfiguration<T>(ETLOptions options)
        {

            if (typeof(T) == typeof(ETLOptions))
            {
                return options;
            }
            string name = typeof(T).Name;
            try
            {
                return options.GetType().GetProperty(name).GetValue(options, null) as Options;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
