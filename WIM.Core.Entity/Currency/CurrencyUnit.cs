﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.Currency
{
    [Table("CurrencyUnit")]
    public class CurrencyUnit
    {
        [Key]
        public int CurrencyIDSys { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public string Country { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }
    }
}
