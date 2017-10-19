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
using WMS.Repository.Impl;
using WMS.Service.Inspect;

namespace WMS.Service.Impl.Inspect
{
    public class InspectService : IInspectService
    {

        private InspectRepository repo;

        public InspectService()
        {
            repo = new InspectRepository ();
        }

        public IEnumerable<InspectType> GetInspectTypes()
        {
            return repo.GetType();
        } 

        public IEnumerable<Inspect_MT> GetInspects()
        {
            return repo.Get();
        }

        public Inspect_MT GetInspectBySupIDSys(int id)
        {
            Inspect_MT Inspect = repo.GetByID(id);
            return Inspect;
        }

        public int CreateInspect(Inspect_MT Inspect)
        {
            using (var scope = new TransactionScope())
            {
                Inspect.CreatedDate = DateTime.Now;
                Inspect.UpdateDate = DateTime.Now;
                Inspect.UserUpdate = "1";
                try
                {
                    repo.Insert(Inspect);
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
                try
                {
                    repo.Update(inspect);

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
                repo.Delete(id);
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
