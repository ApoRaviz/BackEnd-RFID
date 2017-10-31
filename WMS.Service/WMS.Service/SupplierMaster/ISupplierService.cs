﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.SupplierManagement;
using WMS.Common;

namespace WMS.Service
{
    public interface ISupplierService
    {
        IEnumerable<Supplier_MT> GetSuppliers();
        IEnumerable<Supplier_MT> GetSuppliersByProjectID(int projectID);
        Supplier_MT GetSupplierBySupIDSys(int id);
        int CreateSupplier(Supplier_MT Supplier , string username);
        bool UpdateSupplier(Supplier_MT Supplier , string username);
        bool DeleteSupplier(int id);        
    }
}
