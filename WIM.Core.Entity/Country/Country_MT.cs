﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.Currency;

namespace WIM.Core.Entity.Country
{
    [Table("Country_MT")]
    public class Country_MT
    {

        public Country_MT()
        {
            this.CurrencyUnits = new HashSet<CurrencyUnit>();
        }

        [Key]
        public int CountryIDSys { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string PhoneCode { get; set; }
        
        public virtual ICollection<CurrencyUnit> CurrencyUnits { get; set; }
    }
}
