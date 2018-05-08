using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using WMS.Repository;
using System.Data.Entity.Infrastructure;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.Impl;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.UtilityHelpers;
using WMS.Common.ValueObject;

namespace WMS.Service
{
    public class UnitService : WIM.Core.Service.Impl.Service, IUnitService
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
                unit = repo.GetWithInclude(u => u.UnitIDSys == id && u.ProjectIDSys == Identity.GetProjectIDSys(), include).SingleOrDefault();
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

        public int CreateUnit(Unit_MT unit)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IUnitRepository repo = new UnitRepository(Db);
                        unit.ProjectIDSys = Identity.GetProjectIDSys();
                        repo.Insert(unit);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new ValidationException(e);
                }
                catch (DbUpdateException )
                {
                    scope.Dispose();
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
                }
                return unit.UnitIDSys;
            }
        }

        public bool UpdateUnit(Unit_MT unit)
        {           
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IUnitRepository repo = new UnitRepository(Db);
                        unit.ProjectIDSys = Identity.GetProjectIDSys();
                        repo.Update(unit);
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
                    throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
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
                    repo.Delete(x => x.UnitIDSys == id && x.ProjectIDSys == Identity.GetProjectIDSys());
                    Db.SaveChanges();
                    scope.Complete();
                }
                return true;
            }
        }


        public IEnumerable<AutocompleteUnitDto> AutocompleteUnit(string term)
        {
            IEnumerable<AutocompleteUnitDto> autocompleteUnitDto;
            using (WMSDbContext Db = new WMSDbContext())
            {
                UnitRepository repo = new UnitRepository(Db);
                autocompleteUnitDto = repo.AutocompleteUnit(term);

            }
            return autocompleteUnitDto;
        }

    }
}
