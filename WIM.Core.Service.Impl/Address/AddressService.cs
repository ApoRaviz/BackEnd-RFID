
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using WIM.Core.Common.Utility.Validation;
using WIM.Core.Context;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using WIM.Core.Service.Address;

namespace WIM.Core.Service.Impl.StatusManagement
{
    public class AddressService : Service, IAddressService
    {

        public Object GetAddress()
        {
            using (CoreDbContext db = new CoreDbContext())
            {
                ISubCityRepository repoGetAddress = new SubCityRepository(db);
                return repoGetAddress.GetDto();

            }

        }
    }

    


}
