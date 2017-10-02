using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;

namespace WMS.Master.Dimension
{
    public class DimensionService : IDimensionService
    {
        private MasterContext Db = MasterContext.Create();

        public DimensionService()
        {
        }

        public List<DimensionLayout_MT> GetAllDimension()
        {
            List<DimensionLayout_MT> dimension = Db.DimensionLayout_MT.ToList();
            return dimension;
        }

        public List<string> GetColorInSystem(int? id)
        {
            List<DimensionLayout_MT> dimension = Db.DimensionLayout_MT.ToList();
            if (id == null)
                return dimension.Select(x => x.Color).ToList();
            else
                return dimension.Where(x => x.DimensionIDSys != id).Select(x => x.Color).ToList();
        }

        public DimensionLayout_MT GetDimensionLayoutByDimensionIDSys(int id)
        {
            DimensionLayout_MT dimension = Db.DimensionLayout_MT.Find(id);            

            return dimension;
        }

        public int? CreateDimensionOfLocation(DimensionLayout_MT data)
        {
            int? DimensionIDSys = 0;
            using (var scope = new TransactionScope())
            {
                data.CreatedDate = DateTime.Now;
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";
                
                try
                {
                    DimensionIDSys = Db.ProcCreateDimensionLayout(data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                              , data.Type, data.Color, data.CreatedDate, data.UpdatedDate, data.UserUpdate).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();                
            }
            return DimensionIDSys;
        }

        public int? UpdateDimensionOfLocation(int DimensionIDSys, DimensionLayout_MT data)
        {
            int? updateFlag = 0;
            using (var scope = new TransactionScope())
            {
                data.UpdatedDate = DateTime.Now;
                data.UserUpdate = "1";

                try
                {
                    updateFlag = Db.ProcUpdateDimensionLayout(data.DimensionIDSys, data.FormatName, data.Unit, data.Width, data.Length, data.Height, data.Weight
                                              , data.Type, data.Color, data.UpdatedDate, data.UserUpdate).FirstOrDefault();
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
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
            List<DimensionLayout_MT> dimension = Db.DimensionLayout_MT.ToList();

            return dimension;
        }
    }
}
