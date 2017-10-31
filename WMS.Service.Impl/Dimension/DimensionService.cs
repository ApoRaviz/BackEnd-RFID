using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using WIM.Core.Context;
using WIM.Core.Entity.Dimension;
using WMS.Context;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class DimensionService : IDimensionService
    {
        private IIdentity user { get; set; }
        public DimensionService(IIdentity identity)
        {
            user = identity;
        }

        public List<DimensionLayout_MT> GetAllDimension()
        {
            List<DimensionLayout_MT> dimension;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db,user);
                dimension = repo.Get().ToList();
            }

            return dimension;
        }

        public List<string> GetColorInSystem(int? id)
        {
            using (WMSDbContext Db = new WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db,user);
                List<DimensionLayout_MT> dimension = repo.Get().ToList();
                if (id == null)
                    return dimension.Select(x => x.Color).ToList();
                else
                    return dimension.Where(x => x.DimensionIDSys != id).Select(x => x.Color).ToList();
            }
        }

        public DimensionLayout_MT GetDimensionLayoutByDimensionIDSys(int id)
        { DimensionLayout_MT dimension;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db,user);
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
                data.UpdateBy = "1";
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IDimensionRepository repo = new DimensionRepository(Db,user);
                    try
                    {
                        DimensionIDSys = Db.ProcCreateDimensionLayout(data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                                  , data.Type, data.Color, data.CreateAt, data.UpdateAt, data.UpdateBy).FirstOrDefault();
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
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
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IDimensionRepository repo = new DimensionRepository(Db,user);
                    

                    try
                    {
                        updateFlag = Db.ProcUpdateDimensionLayout(data.DimensionIDSys, data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                                  , data.Type, data.Color, data.UpdateAt, data.UpdateBy).FirstOrDefault();
                        Db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        HandleValidationException(e);
                    }
                    scope.Complete();
                }
            }
            return updateFlag;
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

        public List<DimensionLayout_MT> GetBlock()
        {
            List<DimensionLayout_MT> dimension;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IDimensionRepository repo = new DimensionRepository(Db,user);
                dimension =repo.Get().ToList();
            }
            return dimension;
        }
    }
}
