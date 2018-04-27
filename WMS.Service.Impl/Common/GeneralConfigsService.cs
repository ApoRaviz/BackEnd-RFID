using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Service.Impl;
using WMS.Context;
using WMS.Entity.Common;
using WMS.Repository.Common;
using WMS.Repository.Impl.Common;
using WMS.Service.Common;

namespace WMS.Service.Impl.Common
{
    public class GeneralConfigsService : WIM.Core.Service.Impl.Service, IGeneralConfigsService
    {
        //public GeneralConfigs<T> CreateGeneralConfigs(GeneralConfigs config)
        //{
        //    using (var scope = new TransactionScope())
        //    {
        //        GeneralConfigs confignew = new GeneralConfigs();
        //        try
        //        {
        //            using (WMSDbContext Db = new WMSDbContext())
        //            {
        //                IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
        //                confignew = repo.Insert(config);
        //                Db.SaveChanges();
        //                scope.Complete();
        //            }
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw new ValidationException(e);
        //        }
        //        catch (DbUpdateException)
        //        {
        //            scope.Dispose();
        //            ValidationException ex = new ValidationException(ErrorEnum.E4012);
        //            throw ex;
        //        }
        //        return confignew;
        //    }
        //}

        public GeneralConfig CreateGeneralConfigs(GeneralConfig config)
        {
            using (var scope = new TransactionScope())
            {
                GeneralConfig confignew = new GeneralConfig();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                        var res = repo.Insert(config);
                        confignew = AutoMapper.Mapper.Map<BaseGeneralConfig, GeneralConfig>(res);
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
                return confignew;
            }
        }

        public GeneralConfig GetGeneralConfigs(string Keyword)
        {

            GeneralConfig confignew = new GeneralConfig();
            try
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                    BaseGeneralConfig res = repo.GetByID(Keyword);
                    confignew = AutoMapper.Mapper.Map<BaseGeneralConfig, GeneralConfig>(res);
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
            return confignew;
        }

        public GeneralConfigLocationFormat GetLocationFormat()
        {
            GeneralConfigLocationFormat confignew = new GeneralConfigLocationFormat();
            try
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IGeneralConfigsLocationFormatRepository repo = new GeneralConfigsLocationFormatRepository(Db);
                    BaseGeneralConfig res = repo.GetByID("LocationFormat");
                    confignew = AutoMapper.Mapper.Map<BaseGeneralConfig, GeneralConfigLocationFormat>(res);
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
            return confignew;
        }

        public GeneralConfigLocationFormat saveGeneralConfigLocationFormat(GeneralConfigLocationFormat param)
        {
            using (var scope = new TransactionScope())
            {
                GeneralConfigLocationFormat confignew = new GeneralConfigLocationFormat();
               
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IGeneralConfigsLocationFormatRepository repo = new GeneralConfigsLocationFormatRepository(Db);
                        var rs = repo.GetByID(param.Keyword);
                        if ( rs == null)
                        {
                            var x = repo.Insert(param);
                        }
                        else
                        {
                            var x = repo.Update(param);
                        }
                     
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException e)
                {
                    var x = e;
                    scope.Dispose();
                    ValidationException ex = new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                    throw ex;
                }
                return confignew;
            }
        }

        public bool UpdateGeneralConfigs(GeneralConfig config)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IGeneralConfigsRepository repo = new GeneralConfigsRepository(Db);
                        repo.Update(config);
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
                return true;
            }
        }
    }
}
