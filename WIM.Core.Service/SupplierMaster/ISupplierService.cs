using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Service;

namespace WIM.Core.Service
{
    public interface ISupplierService : IService
    {
        IEnumerable<Supplier_MT> GetSuppliers();
        IEnumerable<Supplier_MT> GetSuppliersByProjectID(int projectID);
        Supplier_MT GetSupplierBySupIDSys(int id);
        Supplier_MT CreateSupplier(Supplier_MT Supplier);
        bool UpdateSupplier(Supplier_MT Supplier);
        bool DeleteSupplier(int id);
        IEnumerable<AutocompleteSupplierDto> AutocompleteSupplier(string term);
    }
}
