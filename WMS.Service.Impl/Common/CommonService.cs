using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Service.Common;

namespace WMS.Service.Impl.Common
{
    public class CommonService : WIM.Core.Service.Impl.Service, ICommonService
    {
        public IEnumerable<CheckDependentPKDto> CheckDependentPK(string TableName, string ColumnName, string Value = "")
        {
            IEnumerable<CheckDependentPKDto> checkDependentPKDto = new List<CheckDependentPKDto>();
            try
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    checkDependentPKDto = Db.ProcCheckDependentPK(TableName, ColumnName, Value);
                }
            }
            catch (DbEntityValidationException e)
            {
                throw new ValidationException(e);
            }
            catch (DbUpdateException)
            {
                ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                throw ex;
            }
            return checkDependentPKDto;

        }
    }
}