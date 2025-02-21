﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Master.Master;


namespace Master.Common.ValueObject
{
    public class ItemSetDto 
    {
        public int ItemSetIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string LineID { get; set; }
        public string ItemSetCode { get; set; }
        public string ItemSetName { get; set; }
        public int Active { get; set; }
        public string UserUpdate { get; set; }
        public DateTime UpdateDate { get; set; }

        public ICollection<ItemSetDetailDto> ItemSetDetail { get; set; }

       
    }
}
