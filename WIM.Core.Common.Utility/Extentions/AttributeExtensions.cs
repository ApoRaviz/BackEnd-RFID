using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.Utility.Extentions
{
    public static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var att = type.GetCustomAttributes(
                typeof(TAttribute), true
            ).FirstOrDefault() as TAttribute;
            if (att != null)
            {
                return valueSelector(att);
            }
            return default(TValue);
        }

        public static List<string> GetPropertiesName<TAttribute>(this Type type, object data) where TAttribute : Attribute
        {
            Type t = data.GetType();
            PropertyInfo[] properties = t.GetProperties();
            List<string> propertiesName = new List<string>();
            foreach (PropertyInfo prop in properties)
            {
                TAttribute attr = prop.GetCustomAttribute<TAttribute>();
                if (attr != null)
                {
                    propertiesName.Add(prop.Name);
                }
            }
            return propertiesName;
            throw new Exception("The Object Found KeyAttribute.");
        }       
    }
}
