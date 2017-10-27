using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;

namespace WIM.Core.Repository.Impl
{
    public class ApiMenuMappingRepository : Repository<ApiMenuMapping> , IApiMenuMappingRepository
    {
        private CoreDbContext Db { get; set; }

        public ApiMenuMappingRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }
        //public void Update(ApiMenuMapping entityToUpdate)
        //{
        //    var existed = (from c in Db.ApiMenuMapping
        //                   where c.ApiIDSys == entityToUpdate.ApiIDSys && 
        //                   c.MenuIDSys == entityToUpdate.MenuIDSys
        //                   select c).SingleOrDefault();
        //    existed.ApiIDSys = entityToUpdate.ApiIDSys;
        //    existed.MenuIDSys = entityToUpdate.MenuIDSys;
        //    Db.SaveChanges();
        //}

        
    }
}
