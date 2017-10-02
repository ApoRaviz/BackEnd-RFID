using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WIM.Core.Security.Entity
{
    [Table("Menu_MT")]
    public class Menu
    {
        [Key]
        public int MenuIDSys { get; set; }
        public string Url { get; set; }
        
    }
}