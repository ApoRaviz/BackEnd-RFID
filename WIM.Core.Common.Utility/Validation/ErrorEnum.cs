using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Attributes;

namespace WIM.Core.Common.Utility.Validation
{
    public enum ErrorEnum
    {
        [Description("Data not found!")]
        DataNotFound = 3001,

        [Description("Serial and ItemCode already exists!")]
        ReceiveSerialRemainInStock = 3002,

        [Description("This RFID registered by another Order.")]
        RFIDIsDuplicatedAnother = 3003,

        [Description("RFID not Empty")]
        RFIDNotEmpty = 3004,

        [Description("RFID not Empty")]
        RFIDNeedRepeat = 3005,

        [Description("Cannot connect to printer server!\nPlease check printer and try again!")]
        E1001 = 1001,

        [Description("Login Success"), HttpCode(HttpStatusCode.OK), Action("A0")]
        E2001 = 2001,

        [Description("Cannot login!, Please check network connecting or username/password.")]
        E4001 = 4001,

        [Description("Data not found!")]
        E4004 = 4004,

        [Description("Cannot insert data!, Please check data that have some field invalid"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4011 = 4011,

        [Description("Cannot update data!, Please check data that have in database or not"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4012 = 4012,

        [Description("Your Token Timeout!, Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        E4013 = 4013,

        [Description("Cannot insert data!, Please check you username have used or you have empty data when register"), HttpCode(HttpStatusCode.Conflict)]
        E4009 = 4009,

        [Description("Cannot delete data!, This data has not in system or has been delete."), HttpCode(HttpStatusCode.ExpectationFailed)]
        E4017 = 4017,

        [Description("Service Less than one!"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4112 = 4112,

        [Description("Invalid Parameter in config"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4113 = 4113,

        [Description("Program has a problem.\nPlease contact IT!!!")]
        E5000 = 5000,

        [Description("Your Token Timeout!, Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        E401 = 401,

        [Description("Your Token Timeout!, Cannot POST,PUT,DELETE Please login again."), HttpCode(HttpStatusCode.Unauthorized), Action("A2")]
        E402 = 402,

        [Description("Forbidden!, Not have permission to access this."), HttpCode(HttpStatusCode.Forbidden)]
        E403 = 403,

        [Description("Not Found!, Page cannot be found.")]
        E404 = 404,

        [Description("Request timeout!, Please check network connecting or refresh page.")]
        E408 = 408,

        [Description("Internal Server Error!,Have a promblem on server")]
        E500 = 500,

    }
}
