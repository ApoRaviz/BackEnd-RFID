using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Validation;

public static class EnumExtensions
{
    public static string GetValue<TEnum>(this TEnum value)
    {
        Type genericType = typeof(TEnum);
        if (genericType.IsEnum)
        {
            foreach (TEnum obj in Enum.GetValues(genericType))
            {
                Enum en = Enum.Parse(typeof(TEnum), obj.ToString()) as Enum;
                return Convert.ToInt32(en).ToString();
            }
        }
        return value.ToString();
    }

    public static string GetValueEnum<TEnum>(this TEnum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        ValueEnumAttribute attribute =
            (ValueEnumAttribute)fi.GetCustomAttribute(typeof(ValueEnumAttribute), false);

        if (attribute != null)
            return attribute.Value;
        else
            return value.ToString();
    }

    public static string GetDescription<TEnum>(this TEnum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        DescriptionAttribute attribute =
            (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute), false);

        if (attribute != null)
            return attribute.Description;
        else
            return value.ToString();
    }

    public static HttpStatusCode GetHttpCode<TEnum>(this TEnum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        HttpCodeAttribute[] attributes =
            (HttpCodeAttribute[])fi.GetCustomAttributes(typeof(HttpCodeAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].HttpCode;
        else
            return HttpStatusCode.InternalServerError;
    }

    public static string GetAction<TEnum>(this TEnum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());

        ActionAttribute[] attributes =
            (ActionAttribute[])fi.GetCustomAttributes(typeof(ActionAttribute), false);

        if (attributes != null && attributes.Length > 0)
            return attributes[0].Action;
        else
            return null;
    }
}

