
using System.Security.Principal;
using WIM.Core.Repository.Impl;
using WIM.Core.Security.Context;
using WIM.Core.Security.Entity.UserManagement;

namespace WIM.Core.Security.Repository.Imp
{
    class UserRepository : Repository<User>, IUserRepository
    {
        private SecurityDbContext Db;

        public UserRepository(SecurityDbContext context) : base(context)
        {
            Db = context;
        }

    }
}


