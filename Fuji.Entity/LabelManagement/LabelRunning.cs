﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace Fuji.Entity.LabelManagement
{
    [Table("LabelRunning")]
    public class LabelRunning : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string Type { get; set; }
        //public System.DateTime CreateDate { get; set; }
        public int Running { get; set; }
    }
}
