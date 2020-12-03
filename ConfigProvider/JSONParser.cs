using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConfigProvider
{
    public class JSONParser: IParser
    {

        private readonly Regex keyObjectPattern;
        private readonly Regex keyValuePattern;
        public JSONParser()
        {
            keyObjectPattern = new Regex(@"""(?<ClassName>[^""]*)""\s*:\s*\{(?<Obj>[^}]*)\}", 
                RegexOptions.Compiled |
                RegexOptions.IgnorePatternWhitespace);
            keyValuePattern = new Regex(@"""(?<FieldName>[^""]*)""\s*:\s*[@""]*(?<Value>[^,""]*)\s*[@""]*", 
                RegexOptions.Compiled |
                RegexOptions.IgnorePatternWhitespace);
        }

        public Dictionary<string, object> Parse(string json)
        {
            Dictionary<string, object> objects;

            var trimObj = new Regex(@"^\s*{[\w\W]*}\s*$");
            var match = trimObj.Match(json);
            string input;
            if (match.Success)
            {
                input = match.Value;
            }
            else
            {
                input = json;
            }
            //Console.WriteLine(input);
            objects = ParseObject(input);
            
            return objects;
        }

        private Dictionary<string, object> ParseObject(string jsonObj)
        {
            var ans = new Dictionary<string, object>();

            string key;

            foreach (Match matchClassName in keyObjectPattern.Matches(jsonObj))
            {
                var regClassGroups = matchClassName.Groups;
                key = regClassGroups["ClassName"].Value.ToLower();
                var objFill = regClassGroups["Obj"].Value;
                var objFields = new Dictionary<string, object>();
                foreach (Match matchField in keyValuePattern.Matches(objFill))
                {
                    var regFieldGroups = matchField.Groups;
                    var field = regFieldGroups["FieldName"].Value.ToLower();
                    var value = regFieldGroups["Value"].Value;
                    objFields.Add(field, value);
                }
                ans.Add(key, objFields);
            }

            return ans;
        }
    }
}
