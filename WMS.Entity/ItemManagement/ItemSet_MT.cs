﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.ItemManagement
{
    [Table("ItemSet_MT")]
    public class ItemSet_MT : BaseEntity
    {
        public ItemSet_MT()
        {
            //this.ItemSetDetails = new HashSet<ItemSetDetail>();
        }

        [Key]
        public int ItemSetIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string LineID { get; set; }
        public string ItemSetCode { get; set; }
        public string ItemSetName { get; set; }

        //public virtual Project_MT Project_MT { get; set; }
        public virtual ICollection<ItemSetDetail> ItemSetDetails { get; set; }

        //public virtual ItemSetDetail ItemSetDetail { get; set; }

        //public  ICollection<ItemSetDetail> ItemSetDetails { get; set; }
    }
}
