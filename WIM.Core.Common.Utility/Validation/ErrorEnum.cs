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

        [Description("UNKNOWN_ERROR")]
        UNKNOWN_ERROR = 1010500,

        #endregion /General

        #region ================================ CORE[10] -> Auth[11]

        [Description("INVALID_USERNAME_OR_PASSWORD")]
        InvalidUsernameOrPassword = 1011001,

        [Description("NO_PERMISSION")]
        NO_PERMISSION = 1011002,

        [Description("UNAUTHORIZED"), HttpCode(HttpStatusCode.Unauthorized), Action("A1")]
        UNAUTHORIZED = 1011003,

        [Description("UNAUTHORIZED2"), HttpCode(HttpStatusCode.Unauthorized), Action("A2")]
        UNAUTHORIZED2 = 1011004,

        #endregion /Auth

        #region ================================ CORE[10] -> DATABASE[12]

        [Description("WRITE_DATABASE_PROBLEM"), HttpCode(HttpStatusCode.PreconditionFailed)]
        WRITE_DATABASE_PROBLEM = 1012001,

        [Description("UPDATE_DATABASE_CONCURRENCY_PROBLEM"), HttpCode(HttpStatusCode.ExpectationFailed)]
        UPDATE_DATABASE_CONCURRENCY_PROBLEM = 1012002,

        #endregion /DATABASE

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

        [Description("ORDER_DUPLICATED")]
        ISZJDuplicate = 1411002

        #endregion /Register

        #endregion /ISUZU                    


    }
}
