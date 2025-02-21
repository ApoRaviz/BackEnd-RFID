﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;
using WIM.Core.Entity.ProjectManagement;
using WMS.Entity.ControlMaster;

namespace WMS.Entity.ItemManagement
{
    [Table("Category_MT")]
    public class Category_MT : BaseEntity
    {        
        public Category_MT()
        {
            // # JobComment
            //this.Location_MT = new HashSet<Location_MT>();
        }

        [Key]
        public int CateIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public int? ControlIDSys { get; set; }
        public int? ParentIDSys { get; set; }
        public string CateID { get; set; }
        public string CateName { get; set; }

        [ForeignKey("ProjectIDSys")]
        public virtual Project_MT Project_MT { get; set; }
        [ForeignKey("ControlIDSys")]
        public virtual Control_MT Control_MT { get; set; }
        [ForeignKey("ParentIDSys")]
        public virtual Category_MT MainCategory { get; set; }
        
        //public virtual ICollection<Location_MT> Location_MT { get; set; }
    }
}
