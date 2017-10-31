﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Validation;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InspectionManagement;
using WMS.Repository;
using WMS.Repository.Impl;
using WMS.Service.Inspect;

namespace WMS.Service.Impl.Inspect
{
    public class InspectService : IInspectService
    {
        private IIdentity user { get; set; }
        public InspectService(IIdentity identity)
        {
            user = identity;
        }

        public IEnumerable<InspectType> GetInspectTypes()
        {
            IEnumerable<InspectType> inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IRepository<InspectType> repo = new Repository<InspectType>(Db,user);
                inspect = repo.Get();
            }
                return inspect;
        } 

        public IEnumerable<Inspect_MT> GetInspects()
        {
            IEnumerable<Inspect_MT> inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
               IInspectRepository repo = new InspectRepository(Db,user);
               inspect = repo.Get();
            }
                return inspect;
        }

        public Inspect_MT GetInspectBySupIDSys(int id)
        {
            Inspect_MT Inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IInspectRepository repo = new InspectRepository(Db,user);
                Inspect = repo.GetByID(id);
            }
            return Inspect;
        }

        public int CreateInspect(Inspect_MT Inspect)
        {
            using (var scope = new TransactionScope())
            {
                try
                { 
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IInspectRepository repo = new InspectRepository(Db,user);
                        repo.Insert(Inspect);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                
                return Inspect.InspectIDSys;
            }
        }

        public bool UpdateInspect(Inspect_MT inspect )
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IInspectRepository repo = new InspectRepository(Db,user);
                        repo.Update(inspect);
                        Db.SaveChanges();
                        scope.Complete();
                    }

                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                
                return true;
            }
        }

        public bool DeleteInspect(int id)
        {
            using (var scope = new TransactionScope())
            {

                using (WMSDbContext Db = new WMSDbContext())
                {
                    IInspectRepository repo = new InspectRepository(Db,user);
                    repo.Delete(id);
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
