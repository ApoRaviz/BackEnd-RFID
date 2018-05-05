using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Entity.LabelManagement.LabelConfigs;
using WMS.Context;
using WMS.Entity.SpareField;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service.Impl
{
    public class SpareFieldService : WIM.Core.Service.Impl.Service, ISpareFieldService
    {
        public SpareFieldService( )
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
                int proID = 0;
                List<LabelConfig> labelConfig = new List<LabelConfig>();
            using (var scope = new TransactionScope())
            {
                SpareField SpareFieldnew = new SpareField();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ISpareFieldRepository repo = new SpareFieldRepository(Db);
                        foreach (var x in SpareField)
                        {
                            proID = x.ProjectIDSys;
                            SpareFieldnew = repo.Insert(x);
                            labelConfig.Add(new LabelConfig() { Key = x.Text, Value = x.Text, DefaultValue = x.Text });
                        }
                        if (proID != 0)
                        {
                            WIM.Core.Service.Impl.LabelControlService labelControlService = new WIM.Core.Service.Impl.LabelControlService();
                            labelControlService.AddLabelConfig(proID, labelConfig);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.WRITE_DATABASE_PROBLEM));
                    throw ex;
                }
                return SpareFieldnew.SpfIDSys;
            }
        }

        public bool UpdateSpareField(IEnumerable<SpareField> SpareField)
        {

            int proID = 0;
            List<LabelConfig> labelConfig = new List<LabelConfig>();
            using (var scope = new TransactionScope())
            {

                try
                {
                    //WMSDbContext Db = new WMSDbContext();
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        ISpareFieldRepository repo = new SpareFieldRepository(Db);
                        foreach (var x in SpareField)
                        {
                            if (x.SpfIDSys == 0)
                            {
                                proID = x.ProjectIDSys;
                                repo.Insert(x);
                                labelConfig.Add(new LabelConfig() { Key = x.Text, Value = x.Text, DefaultValue = x.Text });

                            }
                            else
                            {
                                //repo.Update(x);
                            }
                        }
                        if (proID != 0)
                        {
                            WIM.Core.Service.Impl.LabelControlService labelControlService = new WIM.Core.Service.Impl.LabelControlService();
                            labelControlService.AddLabelConfig(proID, labelConfig);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.WRITE_DATABASE_PROBLEM));
                    throw ex;
                }
                //return true;
            }
            return true;
        }

        public bool DeleteSpareField(int id)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {

                        ISpareFieldRepository repo = new SpareFieldRepository(Db);
                        var deactivatedspf = repo.GetByID(id);
                        deactivatedspf.IsActive = false;
                        repo.Update(deactivatedspf);
                        Db.SaveChanges();
                        scope.Complete();

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
                    throw ex;
                }
                return true;
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

}
