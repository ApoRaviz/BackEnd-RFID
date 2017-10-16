using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Security.Entity.RoleAndPermission
{
    [Table("Menu_MT")]
    public class Menu_MT
    {        
        public Menu_MT()
        {
            this.MenuProjectMappings = new HashSet<MenuProjectMapping>();
            //this.ApiMenuMappings = new HashSet<ApiMenuMapping>();
        }

        [Key]
        public int MenuIDSys { get; set; }
        public int MenuParentID { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string Api { get; set; }
        public Nullable<byte> Sort { get; set; }
        public string MenuPic { get; set; }
        public Nullable<byte> MenuProgramID { get; set; }
        public Nullable<byte> Header { get; set; }
        public Nullable<int> MenuGroupID { get; set; }
        public Nullable<int> AjaxMenu { get; set; }
        public Nullable<int> ProjectIDSys { get; set; }
        public Nullable<byte> IsDefault { get; set; }

        public virtual ICollection<MenuProjectMapping> MenuProjectMappings { get; set; }

        //public virtual ICollection<ApiMenuMapping> ApiMenuMappings { get; set; }
    }
}
