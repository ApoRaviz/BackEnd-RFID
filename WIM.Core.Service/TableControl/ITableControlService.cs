using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity.TableControl;

namespace WIM.Core.Service
{
    public interface ITableControlService : IService
    {
        IEnumerable<TableControl> GetTableControl();
    }
}
