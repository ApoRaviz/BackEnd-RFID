using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Repository.ApiMaster;
using WIM.Core.Common.ValueObject;

namespace WIM.Core.Repository.Impl.ApiMaster
{
    public class PermissionGroupRepository : Repository<PermissionGroup>, IPermissionGroupRepository
    {
        private CoreDbContext Db { get; set; }

        public PermissionGroupRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public IEnumerable<PermissionGroup> GetPermissionGroupWithInclude(int MenuIDSys)
        {
            IEnumerable<PermissionGroup> group;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                group = Db.PermissionGroup.Include(a => a.PermissionGroupApi.Select(s => s.Api_MT)).Where(a => a.MenuIDSys == MenuIDSys).ToList().Select(b => new PermissionGroup()
                {
                    MenuIDSys = b.MenuIDSys,
                    GroupIDSys = b.GroupIDSys,
                    GroupName = b.GroupName,
                    IsUpdate = true,
                    PermissionGroupApi = b.PermissionGroupApi.Select(c => new PermissionGroupApi()
                    {
                        GET = c.GET,
                        POST = c.POST,
                        PUT = c.PUT,
                        DEL = c.DEL,
                        Title = c.Title,
                        Api = c.Api_MT.Api,
                        GroupIDSys = c.GroupIDSys,
                        ApiIDSys = c.ApiIDSys,
                        IsUpdate = true
                    }).ToList()
                }).ToList();
            }
            return group;
        }

        public IEnumerable<PermissionTree> GetPermissionByGroupAndMenu(int ProjectIDSys)
        {
            IEnumerable<MenuProjectMapping> group;
            IEnumerable<PermissionTree> tree;
            using (CoreDbContext Db = new CoreDbContext())
            {
                IPermissionGroupRepository repo = new PermissionGroupRepository(Db);
                
                group = Db.MenuProjectMapping.Include(a => a.Menu_MT).Include(a => a.Menu_MT.PermissionGroup.Select(b => b.PermissionGroupApi)).Include(a => a.Permissions).Where(a => a.ProjectIDSys == ProjectIDSys).ToList();
                tree = group.Select(c => new PermissionTree()
                {
                    PermissionID = c.MenuIDSys.ToString(),
                    PermissionName = c.MenuName,
                    Group = c.Menu_MT.PermissionGroup.Select(v => new PermissionTree()
                    {
                        PermissionID = v.GroupIDSys,
                        PermissionName = v.GroupName,
                        Group = c.Permissions.Where(s => v.PermissionGroupApi.Select(q => q.ApiIDSys).Contains(s.ApiIDSys))
                        .Select(w => new PermissionTree()
                        {
                            PermissionID = w.PermissionID,
                            PermissionName = w.PermissionName,
                            Method = w.Method
                        }).ToList()
                    }).ToList()
                }).ToList();

            }
            return tree;
        }
    }
}
