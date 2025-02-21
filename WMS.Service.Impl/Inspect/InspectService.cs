﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WMS.Context;
using WMS.Entity.InspectionManagement;
using WMS.Repository;
using WMS.Repository.Impl;
using WMS.Service.Inspect;

namespace WMS.Service.Impl.Inspect
{
    public class InspectService : WIM.Core.Service.Impl.Service, IInspectService
    {
        public InspectService()
        {
        }

        public IEnumerable<InspectType> GetInspectTypes()
        {
            IEnumerable<InspectType> inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IRepository<InspectType> repo = new Repository<InspectType>(Db);
                inspect = repo.Get();
            }
                return inspect;
        } 

        public IEnumerable<Inspect_MT> GetInspects()
        {
            IEnumerable<Inspect_MT> inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
               IInspectRepository repo = new InspectRepository(Db);
               inspect = repo.Get();
            }
                return inspect;
        }

        public Inspect_MT GetInspectBySupIDSys(int id)
        {
            Inspect_MT Inspect;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IInspectRepository repo = new InspectRepository(Db);
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
                        IInspectRepository repo = new InspectRepository(Db);
                        repo.Insert(Inspect);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
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
                        IInspectRepository repo = new InspectRepository(Db);
                        repo.Update(inspect);
                        Db.SaveChanges();
                        scope.Complete();
                    }

                }
                catch (DbEntityValidationException e)
                {
                    throw new AppValidationException(e);
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
                    IInspectRepository repo = new InspectRepository(Db);
                    repo.Delete(id);
                    scope.Complete();
                }
                return true;
            }
        }

    }
}
