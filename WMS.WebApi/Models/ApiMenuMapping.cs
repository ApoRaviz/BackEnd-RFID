using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WIM.WebApi.Models
{
    [Table("ApiMenuMapping")]
    public class ApiMenuMapping
    {
        public string ApiIDSys { get; set; }
        public int MenuIDSys { get; set; }

        public Api_MT Api { get; set; }
    }
}