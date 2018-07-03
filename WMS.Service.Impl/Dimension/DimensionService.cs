using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.Dimension;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class DimensionService : WIM.Core.Service.Impl.Service, IDimensionService
    {
        public DimensionService()
        {
        }

        public List<DimensionLayout_MT> GetAllDimension()
        {
            List<DimensionLayout_MT> dimension;
            using (Context.WMSDbContext Db = new Context.WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db);
                dimension = repo.Get().ToList();
            }

            return dimension;
        }

        public List<string> GetColorInSystem(int? id)
        {
            using (Context.WMSDbContext Db = new Context.WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db);
                List<DimensionLayout_MT> dimension = repo.Get().ToList();
                if (id == null)
                    return dimension.Select(x => x.Color).ToList();
                else
                    return dimension.Where(x => x.DimensionIDSys != id).Select(x => x.Color).ToList();
            }
        }

        public DimensionLayout_MT GetDimensionLayoutByDimensionIDSys(int id)
        { DimensionLayout_MT dimension;
            using (Context.WMSDbContext Db = new Context.WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db);
                dimension = repo.GetByID(id);
            }

            return dimension;
        }

        public int? CreateDimensionOfLocation(DimensionLayout_MT data)
        {
            int? DimensionIDSys = 0;
            using (var scope = new TransactionScope())
            {
                data.CreateAt = DateTime.Now;
                data.UpdateAt = DateTime.Now;
                data.UpdateBy = Identity.Name;
                using (Context.WMSDbContext Db = new Context.WMSDbContext())
                {
                    IDimensionRepository repo = new DimensionRepository(Db);
                    try
                    {
                        DimensionIDSys = Db.ProcCreateDimensionLayout(data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                                  , data.Type, data.Color, data.CreateAt, data.UpdateAt, data.UpdateBy);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                }

            }
            return DimensionIDSys;
        }

        public int? UpdateDimensionOfLocation(int DimensionIDSys, DimensionLayout_MT data)
        {
            int? updateFlag = 0;
            using (var scope = new TransactionScope())
            {
                using (Context.WMSDbContext Db = new Context.WMSDbContext())
                {
                    IDimensionRepository repo = new DimensionRepository(Db);
                    

                    try
                    {
                        updateFlag = Db.ProcUpdateDimensionLayout(data.DimensionIDSys, data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                                  , data.Type, data.Color, data.UpdateAt, data.UpdateBy);
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new AppValidationException(e);
                    }
                    scope.Complete();
                }
            }
            return updateFlag;
        }

        public List<DimensionLayout_MT> GetBlock()
        {
            List<DimensionLayout_MT> dimension;
            using (Context.WMSDbContext Db = new Context.WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db);
                dimension = repo.Get().ToList();
            }
            return dimension;
        }
    }
}
