using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HashidsNet;
using WIM.Core.Common.Utility.Attributes;

namespace WIM.Core.Common.Utility.Helpers
{
    public class AttributeHelper
    {

        public static void SetHashids(object data)
        {
            if (data == null)
            {
                return;
            }
            Hashids hashids = new Hashids("yut");            
            Type t = data.GetType();
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                HashidsAttrbute attr = prop.GetCustomAttribute<HashidsAttrbute>();
                if (attr == null)
                {
                    continue;
                }
                
                try
                {                    
                    object value = t.GetProperty(prop.Name).GetValue(data, null);
                    var hashValue = hashids.Encode(Convert.ToInt32(value));
                    t.GetProperty(prop.Name).SetValue(data, hashValue, null);                    
                }
                catch (Exception)
                {
                }                
            }

            foreach (PropertyInfo prop in properties)
            {
                HashidsHexsAttrbute attr = prop.GetCustomAttribute<HashidsHexsAttrbute>();
                if (attr == null)
                {
                    continue;
                }

                try
                {
                    List<string> _values = (List<string>)t.GetProperty(prop.Name).GetValue(data, null);
                    List<string> values = new List<string>();
                    foreach (var _value in _values)
                    {
                        string hexValue = hashids.EncodeHex(StringHelper.String2Hex(_value));
                        values.Add(hexValue);
                    }
                 
                    t.GetProperty(prop.Name).SetValue(data, values, null);
                }
                catch (Exception)
                {
                }
            }
        }
        
    }
}
