using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WIM.WebApi.Models
{
    [Table("Api_MT")]
    public class Api_MT
    {
        [Key]
        public string ApiIDSys { get; set; }
        public string Api { get; set; }

    }
}