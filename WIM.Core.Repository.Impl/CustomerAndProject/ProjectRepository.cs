using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Context;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.LabelManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.ProjectManagement;
using WIM.Core.Repository;

namespace WIM.Core.Repository.Impl
{
    public class ProjectRepository : Repository<Project_MT>, IProjectRepository
    {
        private CoreDbContext Db;

        public ProjectRepository(CoreDbContext context) : base(context)
        {
            Db = context;
        }

        public object GetProjectByProjectIDSysInclude(int projectIDSys)
        {
            var query = from pm in Db.Project_MT
                        join mo in Db.Module_MT on pm.ModuleIDSys equals mo.ModuleIDSys
                        where pm.ProjectIDSys == projectIDSys
                        select new
                        {
                            pm.ProjectIDSys,
                            pm.ProjectName,
                            mo.FrontEndPath
                        };
            return query.ToList();
        }

        public List<LabelControl> GetLabelToDuplicate(int moduleIDSys)
        {
            //var labelID = (from i in Db.Module_MT
            //              where i.ModuleIDSys == moduleIDSys
            //              select i.DefaultLang).ToString().Split('|');

            string[] labelID = Db.Module_MT.Where(a => a.ModuleIDSys == moduleIDSys).Select(x => x.DefaultLang).First().Split('|');
            
            List < LabelControl > label = new List<LabelControl>();

            foreach (var i in labelID)
            {
                int id = Convert.ToInt32(i);
                var lb = Db.LabelControl.Where(a => a.LabelIDSys == id).Select(x => x).First();   
                label.Add(lb);
            }

            return label;
        }
    }
}
