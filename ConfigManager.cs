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
        readonly bool jsonConfigParsed = false;
        readonly bool xmlConfigParsed = false;

        readonly ETLOptions defaultConfig;
        readonly ETLJSONOptions jsonConfig;
        readonly ETLXMLOptions xmlConfig;

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
                ConfigManagingInfo += " JSON configuration parsed.";
            }
            catch
            {
                ConfigManagingInfo += " Couldn't parse JSON.";
            }
            try
            {
                using (StreamReader stream = new StreamReader($"{mainPath}\\config.xml"))
                {
                    config = stream.ReadToEnd();
                }
                xmlConfig = new ETLXMLOptions(config);
                xmlConfigParsed = true;
                ConfigManagingInfo += " XML configuration parsed.";
            }
            catch
            {
                ConfigManagingInfo += " Couldn't parse XML.";
            }
            ValidateParsedOptions();
        }

        private void ValidateParsedOptions()
        {
            var configs = new ETLOptions[] { xmlConfig, jsonConfig };
            foreach (var config in configs)
            {
                foreach (var configProperty in config.GetType().GetProperties())
                {
                    foreach (var option in configProperty.PropertyType.GetProperties())
                    {
                        if (option.PropertyType == typeof(string))
                        {
                            var obj = configProperty.GetValue(config);
                            var attrs = option.GetCustomAttributes(false);
                            foreach (var attr in attrs)
                            {
                                if (attr is PathAttribute)
                                {
                                    var path = option.GetValue(configProperty.GetValue(config)) as string;
                                    if (!Directory.Exists(path))
                                    {
                                        option.SetValue(obj, 
                                            option.GetValue(configProperty.GetValue(defaultConfig)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public Options GetConfiguration<T>()
        {
            Options ans;

            if (jsonConfigParsed)
            {
                ans = GetExactConfiguration<T>(jsonConfig);
            }
            else if (xmlConfigParsed)
            {
                ans = GetExactConfiguration<T>(xmlConfig);
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
