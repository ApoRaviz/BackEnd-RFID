﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.ProjectManagement;


namespace WMS.Entity.ItemManagement
{
    [Table("Category_MT")]
    public class Category_MT
    {        
        public Category_MT()
        {
            // # JobComment
            //this.Location_MT = new HashSet<Location_MT>();
        }

        [Key]
        public int CateIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string CateID { get; set; }
        public string CateName { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public virtual Project_MT Project_MT { get; set; }
        
        //public virtual ICollection<Location_MT> Location_MT { get; set; }
    }
}
