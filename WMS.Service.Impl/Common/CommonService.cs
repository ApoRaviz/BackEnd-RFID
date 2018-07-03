using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Repository.Common;
using WMS.Repository.Impl.Common;
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
                throw new AppValidationException(e);
            }
            catch (DbUpdateException)
            {
                AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                throw ex;
            }
            return checkDependentPKDto;

        }

        public string GetValidation(List<string> tableName)
        {
            string validation = "";
            using (WMSDbContext Db = new WMSDbContext())
            {
                ICommonRepository repo = new CommonRepository(Db);
                validation = repo.GetValidation(tableName);
            }
            return validation;
        }
    }
}