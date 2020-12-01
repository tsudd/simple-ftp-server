using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigProvider
{
    public static class ConfigProvader
    {
        public static T Deserialize<T>(string input, IParser parser)
        {
            T ans;

            var type = typeof(T);
            var properties = type.GetProperties();
            var configModel = parser.Parse(input);

            ans = (T)Activator.CreateInstance(type);

            foreach (var option in properties)
            {
                if (configModel.ContainsKey(option.Name.ToLower()))
                {
                    object optionObj = Activator.CreateInstance(option.PropertyType);
                    Dictionary<string, object> optionValues;
                    try
                    {
                        optionValues = (Dictionary<string, object>)Convert.ChangeType(
                            configModel[option.Name.ToLower()], 
                            configModel[option.Name.ToLower()].GetType());
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                    var memberProps = option.PropertyType.GetProperties();
                    foreach (var prop in memberProps)
                    {
                        if (optionValues.ContainsKey(prop.Name.ToLower()))
                        {
                            var value = (string)Convert.ChangeType(optionValues[prop.Name.ToLower()], typeof(string));
                            var valueType = GetMemberType(optionObj.GetType(), prop.Name);
                            object optionObjType;
                            if (valueType.IsEnum)
                            {
                                try
                                {
                                    optionObjType = Enum.Parse(valueType, value);
                                }
                                catch
                                {
                                    optionObjType = null;
                                }

                            }
                            else
                            {
                                optionObjType = Convert.ChangeType(value, valueType);
                            }
                            SetMemberValue(optionObj, prop.Name, optionObjType);
                        }
                    }
                    SetMemberValue(ans, option.Name, optionObj);
                }
            }

            return ans;
        }

        private static void SetMemberValue(object obj, string memberName, object value)
        {
            var objType = obj.GetType();
            if (objType.GetProperty(memberName) != null)
            {
                var info = objType.GetProperty(memberName);
                info.SetValue(obj, value);
            }
            else if (objType.GetField(memberName) != null)
            {
                var info = objType.GetField(memberName);
                info.SetValue(obj, value);
            }
            else
            {
                throw new Exception("This type doesn't contain member with this name");
            }
        }

        private static Type GetMemberType(Type type, string memberName)
        {
            Type type1 = type.GetProperty(memberName)?.PropertyType;
            if (type1 == null)
            {
                type1 = type.GetField(memberName)?.FieldType;
            }
            if (type1 == null)
            {
                throw new Exception("This type doesn't contain member with this name");
            }
            return type1;
        }
    }
}
