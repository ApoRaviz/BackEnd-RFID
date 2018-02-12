using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Common.Utility.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MoreThanOne : ValidationAttribute
    {
        public MoreThanOne()
        {

        }
        protected override ValidationResult IsValid(object value,
                ValidationContext validationContext)
        {
            //try
            //{
                object instance = validationContext.ObjectInstance;
                Type type = instance.GetType();
                IEnumerable<PropertyInfo> properties = type.GetProperties();
                int counttrue = 0;
                bool propertyvalue;
                foreach (PropertyInfo property in properties)
                {

                    if (property.PropertyType == typeof(bool))
                    {
                        propertyvalue = (bool)property.GetValue(instance);
                        if (propertyvalue)
                        {
                            counttrue++;
                        }
                    }
                }
                if (counttrue > 0)
                {
                    return ValidationResult.Success;
                }
                else
                {
                //throw new Exception("");
                //throw new ValidationException(new ValidationError("48888", "Head ไม่เท่ากับที่ Scan รับ"));
                return new ValidationResult
                ("Dont has Service");
            }

            //}
            //catch (ValidationException)
            //{
            //    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorEnum.E4012));
            //    throw ex;
            //}
        }
    }
}
