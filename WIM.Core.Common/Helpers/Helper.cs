using System;
using System.Net;
using System.Reflection;
using WIM.Core.Common.Validation;

namespace WIM.Core.Common.Helpers
{
    public class Helper
    {
        public static string GetHandleErrorMessageException(ErrorCode code, string internalMessage)
        {

            return string.Format("Error #{0}: {1}", (int)code, internalMessage ?? code.GetDescription());
        }
        
        public static ValidationError GetHandleErrorMessageException(ErrorCode code)
        {
            ValidationError error = new ValidationError(code.ToString(), code.GetDescription(), code.GetHttpCode());
            return error;
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
        RFIDNeedRepeat = 3005,

        [ErrorMessage("Cannot connect to printer server!\nPlease check printer and try again!")]
        E1001 = 1001,

        [ErrorMessage("Login Success"), HttpCode(HttpStatusCode.OK), Action("A0")]
        E2001 = 2001,

        [ErrorMessage("Cannot login!, Please check network connecting or username/password.")]
        E4001 = 4001,

        [ErrorMessage("Data not found!")]
        E4004 = 4004,

        [ErrorMessage("Cannot insert data!, Please check data that have some field invalid"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4011 = 4011,

        [ErrorMessage("Cannot update data!, Please check data that have in database or not"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4012 = 4012,

        [ErrorMessage("Your Token Timeout!, Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        E4013 = 4013,

        [ErrorMessage("Cannot insert data!, Please check you username have used or you have empty data when register"), HttpCode(HttpStatusCode.Conflict)]
        E4009 = 4009,

        [ErrorMessage("Cannot delete data!, This data has not in system or has been delete."), HttpCode(HttpStatusCode.ExpectationFailed)]
        E4017 = 4017,

        [ErrorMessage("Program has a problem.\nPlease contact IT!!!")]
        E5000 = 5000,

        [ErrorMessage("Your Token Timeout!, Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        E401 = 401,

        [ErrorMessage("Your Token Timeout!, Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        E402 = 402,

        [ErrorMessage("Forbidden!, Not have permission to access this."), HttpCode(HttpStatusCode.Forbidden)]
        E403 = 403,

        [ErrorMessage("Not Found!, Page cannot be found.")]
        E404 = 404,

        [ErrorMessage("Request timeout!, Please check network connecting or refresh page.")]
        E408 = 408,

        [ErrorMessage("Internal Server Error!,Have a promblem on server")]
        E500 = 500,

    }

    public class ErrorMessageAttribute : Attribute
    {
        public ErrorMessageAttribute(string description)
        {
            this.Description = description;
        }

        public string Description { get; set; }
    }

    public class HttpCodeAttribute : Attribute
    {
        public HttpCodeAttribute(HttpStatusCode httpcode)
        {
            this.HttpCode = httpcode;
        }

        public HttpStatusCode HttpCode { get; set; }
    }

    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string action)
        {
            this.Action = action;
        }

        public string Action { get; set; }
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            ErrorMessageAttribute[] attributes =
                (ErrorMessageAttribute[])fi.GetCustomAttributes(typeof(ErrorMessageAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static HttpStatusCode GetHttpCode(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            HttpCodeAttribute[] attributes =
                (HttpCodeAttribute[])fi.GetCustomAttributes(typeof(HttpCodeAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].HttpCode;
            else
                return HttpStatusCode.InternalServerError;
        }

        public static string GetAction(this Enum value)
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

}
