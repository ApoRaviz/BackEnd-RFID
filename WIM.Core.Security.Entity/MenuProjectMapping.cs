using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WIM.Core.Security.Entity
{
    [Table("MenuProjectMapping")]
    public class MenuProjectMapping
    {     
        public int MenuIDSys { get; set; }
        public int ProjectIDSys { get; set; }      

        public Menu Menu { get; set; }
    }
}