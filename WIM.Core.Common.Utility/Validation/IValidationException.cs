using System;
using System.Collections.Generic;

namespace WIM.Core.Common.Utility.Validation
{
    public interface IValidationException
    {
        IList<ValidationError> Errors { get; set; }
        void Add(ValidationError error);
    }
}
