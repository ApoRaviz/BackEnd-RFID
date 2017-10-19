using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InspectionManagement;
using WMS.Service.Inspect;

namespace WMS.Service.Impl.Inspect
{
    public class InspectService : IInspectService
    {
        private WMSDbContext db = WMSDbContext.Create();
        private GenericRepository<Inspect_MT> repo;

        public InspectService()
        {
            repo = new GenericRepository<Inspect_MT>(db);
        }

        public IEnumerable<InspectType> GetInspectTypes()
        {
            return db.InspectType;
        }

        public IEnumerable<Inspect_MT> GetInspects()
        {
            return repo.GetAll();
        }

        public Inspect_MT GetInspectBySupIDSys(int id)
        {
            Inspect_MT Inspect = db.Inspect_MT.Find(id);
            return Inspect;
        }

        public int CreateInspect(Inspect_MT Inspect)
        {
            using (var scope = new TransactionScope())
            {
                Inspect.CreatedDate = DateTime.Now;
                Inspect.UpdateDate = DateTime.Now;
                Inspect.UserUpdate = "1";

                repo.Insert(Inspect);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return Inspect.InspectIDSys;
            }
        }

        public bool UpdateInspect(int id, Inspect_MT inspect)
        {
            using (var scope = new TransactionScope())
            {
                var existedInspect = repo.GetByID(id);
                existedInspect.InspectName = inspect.InspectName;
                existedInspect.UpdateDate = DateTime.Now;
                existedInspect.UserUpdate = "1";
                repo.Update(existedInspect);
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                scope.Complete();
                return true;
            }
        }

        public bool DeleteInspect(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedInspect = repo.GetByID(id);
                existedInspect.Active = 0;
                existedInspect.UpdateDate = DateTime.Now;
                existedInspect.UserUpdate = "1";
                repo.Update(existedInspect);
                db.SaveChanges();
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
