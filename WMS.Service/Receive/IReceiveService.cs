using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.Receiving;

namespace WMS.Service
{
    public interface IReceiveService : IService
    {
        IEnumerable<Receive> GetReceives();
        Receive GetReceiveByReceiveIDSys(int id);
        int CreateReceive(Receive receive);
        bool UpdateReceive(Receive receive);
        bool DeleteReceive(int id);
        //IEnumerable<AutocompleteUnitDto> AutocompleteUnit(string term);
    }
}
