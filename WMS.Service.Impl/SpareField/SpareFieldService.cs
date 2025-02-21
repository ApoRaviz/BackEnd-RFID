﻿using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.Extensions;
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

        public IEnumerable<SpareField> GetSpareFieldByTableName(string TableName)
        {
            IEnumerable<SpareField> SpareField;
            using (WMSDbContext Db = new WMSDbContext())
            {
                ISpareFieldRepository repo = new SpareFieldRepository(Db);
                SpareField = repo.GetMany(x => x.TableName == TableName && x.ProjectIDSys == Identity.GetProjectIDSys());
            }
            return SpareField;
        }

        //public IEnumerable<SpareFieldDetail> SaveSpareFieldDetail(IEnumerable<SpareFieldsDto> spdDto)
        //{
        //    IEnumerable<SpareFieldDetail> SpareField;

        //    SpareField SpareFieldnew = new SpareField();
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            using (WMSDbContext Db = new WMSDbContext())
        //            {
        //                ISpareFieldDetailRepository repo = new SpareFieldDetailRepository(Db);
        //                repo.insertByDto(spdDto);
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            scope.Dispose();
        //            HandleValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
        //            throw ex;
        //        }
        //    }
        //    return null;
        //}

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
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    AppValidationException ex = new AppValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                        ISpareFieldDetailRepository repos = new SpareFieldDetailRepository(Db);
                        var spareFieldDetail = repos.Get(x => x.SpfIDSys == id);
                        var spareField = repo.GetByID(id);
                        if (spareFieldDetail != null)
                        {
                            spareField.IsActive = false;
                            repo.Update(spareField);
                        }
                        else
                        {
                            repo.Delete(spareField);
                        }
                        Db.SaveChanges();
                        scope.Complete();

                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    AppValidationException ex = new AppValidationException(ErrorEnum.UPDATE_DATABASE_CONCURRENCY_PROBLEM);
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
                    throw new AppValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }
    }

}
