﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace Fuji.Entity.ProgramVersion
{
    [Table("ProgramVersionHistory")]
    public class ProgramVersionHistory : BaseEntity
    {
        [Key]
        public int ID { get; set; }
        public string ProgramName { get; set; }
        public string Version { get; set; }
        public string DownloadPath { get; set; }
        public string Description { get; set; }
    }
}
