
using System.Security.Principal;
using WIM.Core.Repository;
using WIM.Core.Repository.Impl;
using Auth.Security.Context;
using Auth.Security.Entity.UserManagement;

namespace Auth.Security.Repository.Imp
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


