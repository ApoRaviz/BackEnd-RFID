using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Service.Impl
{
    public class HeadReportControlService : Service, IHeadReportControlService
    {
        public HeadReportControlService()
        {

        }

        public IEnumerable<HeadReportControl> GetHeadReportControls()
        {
            IEnumerable<HeadReportControl> report;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                report = repo.Get();
            }
            return report;
        }

        public HeadReportControl GetHeadReportControlByID(int headreportidsys)
        {
            HeadReportControl report;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                report = repo.GetByID(headreportidsys);
            }
            return report;
        }

        public IEnumerable<HeadReportControl> GetHeadReportControlsByModuleID(int SubModuleIDSys)
        {
            IEnumerable<HeadReportControl> report;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                report = repo.GetMany(a => a.SubModuleIDSys == SubModuleIDSys);
            }
            return report;
        }

        public int CreateHeadReportControl(HeadReportControl headreport)
        {
            using (var scope = new TransactionScope())
            {
                HeadReportControl data = new HeadReportControl();
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                        data = repo.Insert(headreport);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return data.HeadReportIDSys;
            }
        }

        public bool UpdateHeadReportControl(HeadReportControl headreport)
        {
            HeadReportControl data = new HeadReportControl();
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                        data = repo.Update(headreport);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
            }
            return true;
        }

        public bool DeleteHeadReportControl(int headreportidsys)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (CoreDbContext Db = new CoreDbContext())
                    {
                        IHeadReportControlRepository repo = new HeadReportControlRepository(Db);
                        HeadReportControl data = repo.GetByID(headreportidsys);
                        repo.Delete(data);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
            }
            return true;
        }

    }
}
