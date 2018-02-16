using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using WIM.Core.Entity.MenuManagement;

namespace WIM.Core.Repository
{
    public interface IMenuRepository : IRepository<Menu_MT>
    {
        IEnumerable<AutocompleteMenuDto> AutocompleteMenu(string term);
    }
}
