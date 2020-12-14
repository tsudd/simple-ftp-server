using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace DataManager
{
    public class XMLGeneratorService: IXMLGenerator
    {
        private readonly string generationPlace;
        public XMLGeneratorService(string place)
        {
            if (!Directory.Exists(place))
            {
                throw new ArgumentException();
            }
            generationPlace = place;
        }

        public string GenerateXML(object data, string fileName = "data-pull")
        {
            var serialazer = new XmlSerializer(data.GetType());
            var xmlPath = generationPlace + "\\" + fileName.Split('.')[0] + DateTime.Now.ToString("hh_mm_ss") + ".xml";
            using (var fs = new FileStream(xmlPath, FileMode.OpenOrCreate))
            {
                serialazer.Serialize(fs, data);
            }
            return xmlPath;
        }

    }
}
