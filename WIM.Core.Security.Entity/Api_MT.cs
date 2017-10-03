using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WIM.Core.Security.Entity
{
    [Table("Api_MT")]
    public class Api_MT
    {
        [Key]
        public string ApiIDSys { get; set; }
        public string Api { get; set; }

    }
}