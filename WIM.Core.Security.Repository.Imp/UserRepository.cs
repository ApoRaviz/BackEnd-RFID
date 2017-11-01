
using System.Security.Principal;
using WIM.Core.Repository.Impl;
using WIM.Core.Security.Context;
using WIM.Core.Security.Entity.UserManagement;

namespace WIM.Core.Security.Repository.Imp
{
    class UserRepository : Repository<User>, IUserRepository
    {
        private SecurityDbContext Db;
        //private ItemSetRepository repo;
        private IIdentity Identity;

        public UserRepository(SecurityDbContext context, IIdentity identity) : base(context, identity)
        {
            Db = context;
        }

    }
}


