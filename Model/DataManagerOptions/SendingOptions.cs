using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DataManagerOptions
{
    public enum PullMode
    {
        FullTable,
        ByPackages,
        ByJoin
    }
    public class SendingOptions: Options
    {
        [Path]
        public string SendingPlace { get; set; } = "F:\\From the Internet";
        public PullMode PullMode { get; set; } = PullMode.FullTable;
        public int PackageSize { get; set; } = 100;

        public SendingOptions()
        {

        }
    }
}
