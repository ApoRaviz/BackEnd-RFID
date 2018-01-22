using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.ProjectManagement.ProjectConfigs;
using WIM.Core.Entity.RoleAndPermission;
//using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Entity.ProjectManagement
{
    [Table("Project_MT")]
    public class Project_MT : BaseEntity
    {
        public Project_MT()
        {
            this.Menu_MT = new HashSet<Menu_MT>();
            this.MenuProjectMappings = new HashSet<MenuProjectMapping>();
            this.Roles = new HashSet<Role>();
        }

        [Key]
        public int ProjectIDSys { get; set; }
        public string ProjectName { get; set; }
        public int CusIDSys { get; set; }
        public Nullable<int> ModuleIDSys { get; set; }
        public string ProjectStatus { get; set; }
        public string Config { get; private set; }

        [NotMapped]
        public ProjectConfig ProjectConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<ProjectConfig>(StringHelper.Decompress(Config));
                }
                return null;
            }
            set
            {
                Config = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }

        public virtual ICollection<Menu_MT> Menu_MT { get; set; }

        public virtual ICollection<MenuProjectMapping> MenuProjectMappings { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        [ForeignKey("CusIDSys")]
        public virtual Customer_MT Customer_MT { get; set; }


    }       
}
