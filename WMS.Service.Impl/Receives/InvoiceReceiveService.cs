
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using WIM.Core.Common.Utility.Validation;
using WMS.Common.ValueObject;
using WMS.Context;
using WMS.Entity.Receiving;
using WMS.Repository;
using WMS.Repository.Impl;

namespace WMS.Service.Impl.Receives
{
    public class InvoiceReceiveService : WIM.Core.Service.Impl.Service, IInvoiceReceiveService
    {
        public InvoiceReceivesDto CreateInvReceives(InvoiceReceive param)
        {
            try
            {
                using (WMSDbContext Db = new WMSDbContext())
                {
                    IInvoiceReceiveRepository InvRepo = new InvoiceReceiveRepository(Db);
                    InvRepo.Insert(param);
                    Db.SaveChanges();
                    if(param.ReceiveIDSys != null)
                    {
                        InvRepo.GetInvReceivesDto(param.ReceiveIDSys);
                    }
                    else
                    {

                    }
                    return null;
                }
            }
            catch (DbEntityValidationException)
            {
                throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
            }
            catch (DbUpdateException)
            {
                throw new ValidationException(ErrorEnum.WRITE_DATABASE_PROBLEM);
            }
            
            
        }



    }
}
