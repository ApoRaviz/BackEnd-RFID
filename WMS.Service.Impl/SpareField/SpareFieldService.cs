using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.SpareField;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service.Impl
{
    public class SpareFieldService : WIM.Core.Service.Impl.Service, ISpareFieldService
    {
        public SpareFieldService()
        {
        }

        public IEnumerable<SpareField> GetSpareField()
        {
            IEnumerable<SpareField> SpareField;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ISpareFieldRepository repo = new SpareFieldRepository(Db);
                SpareField = repo.Get();
            }
            return SpareField;
        }

        public SpareField GetSpareFieldBySpfIDSys(int id)
        {
            SpareField SpareField;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ISpareFieldRepository repo = new SpareFieldRepository(Db);
                SpareField = repo.GetByID(id);
            }
            return SpareField;
        }

        public IEnumerable<SpareField> GetSpareFieldByProjectIDSys(int id)
        {
            IEnumerable<SpareField> SpareField;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ISpareFieldRepository repo = new SpareFieldRepository(Db);
                SpareField = repo.GetMany(x => x.ProjectIDSys == id);
            }
            return SpareField;
        }

        public int CreateSpareField(IEnumerable<SpareField> SpareField)
        {
            using (var scope = new TransactionScope())
            {
                SpareField SpareFieldnew = new SpareField();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ISpareFieldRepository repo = new SpareFieldRepository(Db);
                        foreach(var x in SpareField)
                        {
                        SpareFieldnew = repo.Insert(x);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return SpareFieldnew.SpfIDSys;
            }
        }

        public bool UpdateSpareField(IEnumerable<SpareField> SpareField)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ISpareFieldRepository repo = new SpareFieldRepository(Db);
                        foreach (var x in SpareField)
                        {
                        repo.Update(x);
                        }
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return true;
            }
        }

        public bool DeleteSpareField(int id)
        {
            throw new NotImplementedException();
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

}
