using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Main
{
    public class Service
    {
        //[Custom]
        public bool IsImport { get; set; }
        public bool IsWarehouse { get; set; }
        public bool IsPacking { get; set; }
        public bool IsTransport { get; set; }
    }

    public class CustomAttribute : ValidationAttribute
    {
        private readonly string _other;
        public CustomAttribute()
        {
            //_other = other;
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return new ValidationResult(this.FormatErrorMessage("xxxx ^ ^"));
            var property = validationContext.ObjectType.GetProperty(_other);
            if (property == null)
            {
                return new ValidationResult(
                    string.Format("Unknown property: {0}", _other)
                );
            }
            var otherValue = property.GetValue(validationContext.ObjectInstance, null);

            // at this stage you have "value" and "otherValue" pointing
            // to the value of the property on which this attribute
            // is applied and the value of the other property respectively
            // => you could do some checks
            if (!object.Equals(value, otherValue))
            {
                // here we are verifying whether the 2 values are equal
                // but you could do any custom validation you like
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
            return null;
        }
    }

}
