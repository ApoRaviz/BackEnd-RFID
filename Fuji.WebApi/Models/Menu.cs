using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Fuji.WebApi.Models
{
    [Table("Menu_MT")]
    public class Menu
    {
        [Key]
        public int MenuIDSys { get; set; }
        public string Url { get; set; }
        
    }
}