using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public interface IXMLGenerator
    {
        string GenerateXML(object objArg, string fileName);
    }
}
