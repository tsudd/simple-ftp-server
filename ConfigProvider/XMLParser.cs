using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConfigProvider
{
    public class XMLParser: IParser
    {
        readonly Regex rootReg;
        readonly Regex classContentPattern;
        readonly Regex fieldValuePattern;
        public XMLParser()
        {
            rootReg = new Regex(@"<(?<AllConfName>[^>]*)>\s*(?<Content>[\w\W]*)\s*<(/\k<AllConfName>[\w\W]*)>", 
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            classContentPattern = new Regex(@"\s*<(?<ClassName>[^>]*)>\s*(?<Content>[\w\W]*)\s*</\k<ClassName>>", 
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
            fieldValuePattern = new Regex(@"\s*<(?<FieldName>[^>]*)>\s*(?<Value>[\w\W]*)\s*</\k<FieldName>>", 
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        }
        public Dictionary<string, object> Parse(string xml)
        {
            var match = rootReg.Match(xml);
            string input;
            if (match.Success)
            {
                input = match.Groups["Content"].Value;
            }
            else
            {
                throw new ArgumentException();
            }

            Console.WriteLine(input);
            var objects = ParseObject(input);
            return objects;
        }

        private Dictionary<string, object> ParseObject(string xmlObj)
        {
            var ans = new Dictionary<string, object>();

            string tag;

            foreach (Match matchClassWithContent in classContentPattern.Matches(xmlObj))
            {
                var optionGroups = matchClassWithContent.Groups;
                tag = optionGroups["ClassName"].Value.ToLower();
                var content = optionGroups["Content"].Value;
                var optionObject = new Dictionary<string, object>();
                foreach(Match matchFieldValue in fieldValuePattern.Matches(content))
                {
                    var fieldGroups = matchFieldValue.Groups;
                    var field = fieldGroups["FieldName"].Value.ToLower();
                    var value = fieldGroups["Value"].Value;
                    optionObject.Add(field, value);
                }
                ans.Add(tag, optionObject);
            }
            return ans;
        }
    }
}
