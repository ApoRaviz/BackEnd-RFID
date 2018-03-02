﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.Address;

namespace WIM.Core.Repository
{
    public interface ISubCityRepository : IRepository<SubCity_MT>
    {
        Object GetDto();
    }
}
