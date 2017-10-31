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
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.Impl;


namespace WMS.Service
{
    public class UnitService : IUnitService
    {
        public UnitService()
        {
        }        
        public IEnumerable<Unit_MT> GetUnits()
        {
            IEnumerable<Unit_MT> unit;
            using(WMSDbContext Db = new WMSDbContext())
            {
                IUnitRepository repo = new UnitRepository(Db);
                unit = repo.Get();
            }
            return unit;
        }

        public Unit_MT GetUnitByUnitIDSys(int id)
        {
            Unit_MT unit;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IUnitRepository repo = new UnitRepository(Db);
                string[] include = { "Project_MT" };
                unit = repo.GetWithInclude(u => u.UnitIDSys == id, include).SingleOrDefault();
            }
                return unit;
        }

        public Unit_MT GetUnitByCusIDSysIncludeProjects(int id)
        {
            var unit = GetUnitByUnitIDSys(id);
            if (unit != null)
            {
                //return unit;
                //Mapper.Initialize(cfg => cfg.CreateMap<Unit_MT, UnitDto>());
                //UnitDto unitsDto = Mapper.Map<Unit_MT, UnitDto>(unit);
                return unit;
            }
            return null;
        }

        public int CreateUnit(Unit_MT unit,string username)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IUnitRepository repo = new UnitRepository(Db);
                        repo.Insert(unit , username);
                        Db.SaveChanges();
                        scope.Complete();
                    }
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
                return unit.UnitIDSys;
            }
        }

        public bool UpdateUnit(Unit_MT unit,string username)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IUnitRepository repo = new UnitRepository(Db);
                        repo.Update(unit, username);
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
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                
                return true;
            }
        }

        public bool DeleteUnit(int id)
        {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IUnitRepository repo = new UnitRepository(Db);
                    repo.Delete(id);
                    Db.SaveChanges();
                    scope.Complete();
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
