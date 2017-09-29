using System;
using System.Collections.Generic;

namespace WIM.Core.Common.Validation
{
    public interface IValidationException
    {
        IList<ValidationError> Errors { get; set; }
        void Add(ValidationError error);
    }
}
