using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class EncryptionOptions : Options
    {
        public bool EnableEncryption { get; set; } = true;

        public EncryptionOptions()
        {

        }
    }
}
