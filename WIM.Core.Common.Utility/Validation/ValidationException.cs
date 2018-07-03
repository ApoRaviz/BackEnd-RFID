
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;

namespace WIM.Core.Common.Utility.Validation
{
    public class AppValidationException : Exception, IValidationException
    {
        public AppValidationException() : base()
        {
            this.Errors = new List<ValidationError>();
        }

        public AppValidationException(string key) : base()
        {
            this.Errors = new List<ValidationError>();
            this.Add(new ValidationError(key, string.Empty));
        }

        public AppValidationException(ValidationError error) : base()
        {
            this.Errors = new List<ValidationError>();
            this.Add(error);
        }

        public AppValidationException(string key, params object[] args) : this()
        {
            IList<string> extParam = new List<string>();
            foreach (object param in args)
            {
                extParam.Add(param.ToString());
            }
            this.Add(new ValidationError(key, string.Empty, extParam));
        }

        public AppValidationException(ErrorEnum errorEnum) : base()
        {
            this.Errors = new List<ValidationError>();
            this.Add(new ValidationError(errorEnum));
        }

        public AppValidationException(ErrorEnum errorEnum, string message) : base()
        {
            this.Errors = new List<ValidationError>();
            this.Add(new ValidationError(errorEnum, message));
        }

        public AppValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new AppValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

        public IList<ValidationError> Errors { get; set; }

        public void Add(ValidationError error)
        {
            this.Errors.Add(error);
        }

        public void Add(IList<ValidationError> errors)
        {
            foreach (var validationError in errors)
            {
                this.Add(validationError);
            }
        }
    }

    public class EntityException : Exception
    {
        public IEnumerable<ValidationResult> Errors { get; set; }

        public EntityException(IEnumerable<ValidationResult> errors)
        {
            this.Errors = errors;
        }

        public EntityException(ValidationResult error)
        {
            this.Errors = new List<ValidationResult>() { error };
        }
    }
}