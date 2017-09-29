using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Common.Validation;
using WIM.Repositories;

namespace WIM.ExternallInterface
{
    public class HelperService : IHelperService
    {
        private WIM_FUJI_DEVEntities db;

        public HelperService()
        {
            db = new WIM_FUJI_DEVEntities();
        }

        public void InsertErrorLog(ErrorLogs errorLog)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    errorLog.CreateAt = DateTime.Now;
                    db.ErrorLogs.Add(errorLog);
                    db.SaveChanges();
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }            
        }
    }

    public enum ErrorCode
    {

        [ErrorMessage("Data not found!")]
        DataNotFound = 3001,

        [ErrorMessage("")]
        ReceiveSerialRemainInStock = 3002,

        [ErrorMessage("")]
        RFIDIsDuplicatedAnother = 3003,

        [ErrorMessage("RFID not Empty")]
        RFIDNotEmpty = 3004,

        [ErrorMessage("RFID not Empty")]
        RFIDNeedRepeat = 3005

    }

    public class ErrorMessageAttribute : Attribute
    {

        public ErrorMessageAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ErrorMessageAttribute[] attributes =
                (ErrorMessageAttribute[])fi.GetCustomAttributes(typeof(ErrorMessageAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }                
        }
    }
}
