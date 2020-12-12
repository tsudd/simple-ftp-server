using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataManagerOptions
{
    public class ConnectionOptions: Options
    {
        public string DataSource { get; set; } = "TSUDDBOOK\\AdventureWorks";
        public string Database { get; set; } = "AdventureWorks2017";
        public string User { get; set; } = "NoNameUser";
        public string Password { get; set; } = "qwerty123";
        public bool IntegratedSecurity { get; set; } = false;
        public ConnectionOptions()
        {

        }
    }
}
