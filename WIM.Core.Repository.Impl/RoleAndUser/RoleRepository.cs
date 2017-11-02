using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        private CoreDbContext Db { get; set; }

        public RoleRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public string GetByUserAndProject(string UserID, int ProjectIDSys)
        {
            var res = (from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID && r.ProjectIDSys == ProjectIDSys
                       select new { r.RoleID }).SingleOrDefault();
            return res.RoleID;
        }

        public List<Role> GetByProjectUser(int id)
        {
            var customer = (from row in Db.Project_MT
                            where row.ProjectIDSys == id
                            select row.CusIDSys).SingleOrDefault();
            var role = (from row in Db.Role
                        join row2 in Db.RolePermission on row.RoleID equals row2.RoleID
                        join row3 in Db.Permission on row2.PermissionID equals row3.PermissionID
                        join row4 in Db.Project_MT on row3.ProjectIDSys equals row4.ProjectIDSys
                        where row4.CusIDSys == customer
                        select row).Include("Project_MT").Distinct().ToList();

            return role;
        }

        public List<Role> GetByUser(string UserID)
        {
            var res = (from ur in Db.UserRoles
                       join r in Db.Role on ur.RoleID equals r.RoleID
                       where ur.UserID == UserID
                       select r).Include(b => b.Project_MT).ToList();
            return res;
        }


    }
}
