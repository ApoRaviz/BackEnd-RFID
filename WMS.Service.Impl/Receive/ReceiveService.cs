using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.Receiving;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service.Impl
{
    public class ReceiveService : WIM.Core.Service.Impl.Service, IReceiveService
    {
        public IEnumerable<Receive> GetReceives()
        {
            IEnumerable<Receive> receives;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReceiveRepository repo = new ReceiveRepository(Db);
                receives = repo.Get();
            }
            return receives;
        }

        public Receive GetReceiveByReceiveIDSys(int id)
        {
            Receive unit;
            using (WMSDbContext Db = new WMSDbContext())
            {
                IReceiveRepository repo = new ReceiveRepository(Db);
                unit = repo.GetWithInclude(u => u.ReceiveIDSys == id).SingleOrDefault();
            }
            return unit;
        }

        public int CreateReceive(Receive receive)
        {
            using (var scope = new TransactionScope())
            {
                Receive newReceive = new Receive();
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IReceiveRepository repo = new ReceiveRepository(Db);
                        newReceive = repo.Insert(receive);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }
                return newReceive.ReceiveIDSys;
            }
        }

        public bool UpdateReceive(Receive receive)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    using (WMSDbContext Db = new WMSDbContext())
                    {
                        IReceiveRepository repo = new ReceiveRepository(Db);
                        repo.Update(receive);
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
                    ValidationException ex = new ValidationException(UtilityHelper.GetHandleErrorMessageException(ErrorEnum.E4012));
                    throw ex;
                }

                return true;
            }
        }

        public bool DeleteReceive(int id)
        {
            throw new NotImplementedException();
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
