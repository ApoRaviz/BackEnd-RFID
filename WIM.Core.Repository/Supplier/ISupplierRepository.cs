using System;
using System.Collections.Generic;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.SupplierManagement;

namespace WIM.Core.Repository.Supplier
{
    public interface ISupplierRepository : IRepository<Supplier_MT>
    {
        IEnumerable<AutocompleteSupplierDto> AutocompleteSupplier(string term);
    }
}
