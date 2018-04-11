using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WIM.Core.Common.Utility.Validation;
using WMS.Context;
using WMS.Entity.ControlMaster;
using WMS.Repository.ControlMaster;
using WMS.Repository.Impl.ControlMaster;
using WMS.Service.ControlMaster;

namespace WMS.Service.Impl.ControlMaster
{
    public class ControlService : WIM.Core.Service.Impl.Service, IControlService
    {
        public ControlService()
        {

        }

        public IEnumerable<Control_MT> GetControl()
        {
            IEnumerable<Control_MT> controls;
            using (WMSDbContext Db = new WMSDbContext())
            {

                IControlRepository repo = new ControlRepository(Db);
                controls = repo.Get();
            }
            return controls;
        }

        public Control_MT GetControl(int id)
        {
            Control_MT control;
            using (WMSDbContext Db = new WMSDbContext())
            {

                IControlRepository repo = new ControlRepository(Db);
                control = repo.GetByID(id);
            }
            return control;
        }

        public int CreateControl(Control_MT control)
        {
            using (var scope = new TransactionScope())
            {
                Control_MT x;
                using (WMSDbContext Db = new WMSDbContext())
                {

                    IControlRepository repo = new ControlRepository(Db);
                    try
                    {
                        x = repo.Insert(control);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        throw new ValidationException(ErrorEnum.E4012);
                    }
                }
                return x.ControlIDSys;
            }
        }

            public bool UpdateControl(Control_MT control)
            {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {

                    IControlRepository repo = new ControlRepository(Db);
                    try
                    {
                        repo.Update(control);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbEntityValidationException e)
                    {
                        throw new ValidationException(e);
                    }
                    catch (DbUpdateException)
                    {
                        scope.Dispose();
                        throw new ValidationException(ErrorEnum.E4012);

                    }
                }
                return true;
            }
        }

            public bool DeleteControl(int id)
            {
            using (var scope = new TransactionScope())
            {
                using (WMSDbContext Db = new WMSDbContext())
                {

                    IControlRepository repo = new ControlRepository(Db);
                    var existedControl = repo.GetByID(id);
                    try
                    {
                        existedControl.IsActive = false;
                        repo.Delete(existedControl);
                        Db.SaveChanges();
                        scope.Complete();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        scope.Dispose();
                        throw new ValidationException(ErrorEnum.E4017);
                    }

                }
                return true;
            }
        }


    }
}
