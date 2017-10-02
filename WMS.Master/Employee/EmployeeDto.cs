﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Master;
using WMS.Master.Unit;
using WMS.Repository;

namespace WMS.Master
{
    public class EmployeeDto : BaseEntityDto
    {
        public string EmID { get; set; }
        public int PersonIDSys { get; set; }
        public string Area { get; set; }
        public string Position { get; set; }
        public string Dept { get; set; }
        public string TelOffice { get; set; }
        public string TelEx { get; set; }
        public byte Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string UserUpdate { get; set; }

        public  Person_MT Person_MT { get; set; }
    }
}
