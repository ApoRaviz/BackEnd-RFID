using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.CustomerManagement;
using WIM.Core.Entity.MenuManagement;
using WIM.Core.Entity.RoleAndPermission;
using WIM.Core.Entity.SupplierManagement;
//using WIM.Core.Security.Entity.RoleAndPermission;

namespace WIM.Core.Entity.ProjectManagement
{
    [Table("Project_MT")]
    public class Project_MT : BaseEntity
    {
        public Project_MT()
        {
            this.Supplier_MT = new HashSet<Supplier_MT>();
            this.Menu_MT = new HashSet<Menu_MT>();
            this.MenuProjectMappings = new HashSet<MenuProjectMapping>();

            // #JobComment
            //this.UserProjectMappings = new HashSet<UserProjectMapping>();
            this.Roles = new HashSet<Role>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int ProjectIDSys { get; set; }
        public string ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int CusIDSys { get; set; }
        public Nullable<int> ModuleIDSys { get; set; }
        public string ProjectStatus { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //public System.DateTime UpdateDate { get; set; }
        //public string UserUpdate { get; set; }

        public virtual ICollection<Supplier_MT> Supplier_MT { get; set; }

        public virtual ICollection<Menu_MT> Menu_MT { get; set; }

        public virtual ICollection<MenuProjectMapping> MenuProjectMappings { get; set; }

        //public virtual ICollection<UserProjectMapping> UserProjectMappings { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public virtual Customer_MT Customer_MT { get; set; }
    }
}
