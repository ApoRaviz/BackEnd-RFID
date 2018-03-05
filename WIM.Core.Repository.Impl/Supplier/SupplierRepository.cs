using System.Collections.Generic;
using System.Linq;
using WIM.Core.Common.ValueObject;
using WIM.Core.Context;
using WIM.Core.Entity.SupplierManagement;
using WIM.Core.Repository.Supplier;

namespace WIM.Core.Repository.Impl.Supplier
{
    public class SupplierRepository : Repository<Supplier_MT> , ISupplierRepository
    {
        private CoreDbContext Db { get; set; }
        public SupplierRepository(CoreDbContext context):base(context)
        {
            Db = context;
        }

        public IEnumerable<AutocompleteSupplierDto> AutocompleteSupplier(string term)
        {
            var qr = (from sp in Db.Supplier_MT
                      where sp.SupID.Contains(term)
                      || sp.CompName.Contains(term)
                      select new AutocompleteSupplierDto
                      {
                          SupIDSys = sp.SupIDSys,
                          SupID = sp.SupID,
                          CompName = sp.CompName
                      }
            ).ToList();
            return qr;
        }

        
    }
}
