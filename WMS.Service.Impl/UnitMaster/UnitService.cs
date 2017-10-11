using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Master;

namespace WMS.Service
{
    public class UnitService : IUnitService
    {
        private MasterContext Db = MasterContext.Create();
        private GenericRepository<Unit_MT> Repo;

        public UnitService()
        {
            Repo = new GenericRepository<Unit_MT>(Db);
        }        

        public IEnumerable<ProcGetUnits_Result> GetUnits()
        {
            return Db.ProcGetUnits().ToList();
        }

        public ProcGetUnitByUnitIDSys_Result GetUnitByUnitIDSys(int id)
        {
            return Db.ProcGetUnitByUnitIDSys(id).FirstOrDefault();
        }

        public Unit_MT GetUnitByCusIDSysIncludeProjects(int id)
        {
            var unit = Repo.GetByID(id);
            if (unit != null)
            {
                //return unit;
                //Mapper.Initialize(cfg => cfg.CreateMap<Unit_MT, UnitDto>());
                //UnitDto unitsDto = Mapper.Map<Unit_MT, UnitDto>(unit);
                return unit;
            }
            return null;
        }

        public int CreateUnit(Unit_MT unit)
        {
            using (var scope = new TransactionScope())
            {

                unit.CreatedDate = DateTime.Now;
                unit.UpdateDate = DateTime.Now;
                unit.UserUpdate = "1";

                Repo.Insert(unit);
                try
                {
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return unit.UnitIDSys;
            }
        }

        public bool UpdateUnit(int id, Unit_MT unit)
        {           
            using (var scope = new TransactionScope())
            {
                var existedUnit = Repo.GetByID(id);
                existedUnit.UnitName = unit.UnitName;
                existedUnit.UpdateDate = DateTime.Now;
                existedUnit.UserUpdate = "1";
                Repo.Update(existedUnit);
                try
                {
                    Db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                scope.Complete();
                return true;
            }
        }

        public bool DeleteUnit(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedUnit = Repo.GetByID(id);
                existedUnit.Active = 0;
                existedUnit.UpdateDate = DateTime.Now;
                existedUnit.UserUpdate = "1";
                Repo.Update(existedUnit);
                Db.SaveChanges();
                scope.Complete();
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
