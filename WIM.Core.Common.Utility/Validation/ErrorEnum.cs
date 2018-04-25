using System;
using System.Net;
using WIM.Core.Common.Utility.Http;
using WIM.Core.Common.Utility.Attributes;

namespace WIM.Core.Common.Utility.Validation
{    

    public enum ErrorEnum
    {
        #region ================================ CORE[10] ======================

        #region ================================ CORE[10] -> General[10]

        [Description("DATA_NOT_FOUND")]
        DataNotFound = 1010001,

        #endregion /General

        #region ================================ CORE[10] -> Auth[11]

        [Description("DATA_NOT_FOUND")]
        InvalidUsernameOrPassword = 1011001,

        [Description("NO_PERMISSION")]
        NoPermission = 1011002,

        #endregion /Auth

        [Description("WRITE_DATABASE_PROBLEM"), HttpCode(HttpStatusCode.PreconditionFailed)]
        E4012 = 4012,


        #endregion /CORE





        #region ================================ MASTER[11] ======================

        #endregion /MASTER





        #region ================================ WMS[12] =========================

        #endregion /WMS





        #region ================================ FUJI[13] =========================


        #region ================================ FUJI[13] -> Receive[12]

        [Description("SERIAL_AND_ITEMCODE_ALREADY_EXISTS")]
        SerialAndItemCodeAlreadyExist = 1312001,

        #endregion /Receive



        #endregion /FUJI


        #region ================================ ISUZU[14] =========================

        #region ================================ ISUZU[14] -> Register[11]

        [Description("RFID_DUPLICATED")]
        RFIDDuplicated = 1411001,

        #endregion /Register

        #endregion /ISUZU                    

        

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

        [Description("Program has a problem.\nPlease contact IT!!!")]
        E500 = 500
    }
}
