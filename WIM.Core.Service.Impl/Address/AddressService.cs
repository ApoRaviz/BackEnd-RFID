using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using WIM.Core.Context;
using WIM.Core.Entity.Status;
using WIM.Core.Common.ValueObject;
using WIM.Core.Common;
using WIM.Core.Repository.Impl.StatusManagement;
using WIM.Core.Repository.StatusManagement;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Service.Address;
using WIM.Core.Repository;
using System;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Service.Impl.StatusManagement
{
    public class AddressService : Service, IAddressService
    {

        public Object GetAddress()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                try
                {
                    ISubCityRepository repoGetLeave = new SubCityRepository(db);
                    return repoGetLeave.GetDto();
                }

                catch (DbEntityValidationException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }

                catch (DbUpdateException)
                {
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
            }

        }
    }

    


}
