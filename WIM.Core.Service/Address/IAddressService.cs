using System;
using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Status;


namespace WIM.Core.Service.Address
{
    public interface IAddressService : IService
    {
        Object GetAddress();
    }
}
