using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Repository;
using WMS.Common.ValueObject;
using WMS.Entity.Receiving;

namespace WMS.Repository
{
    public interface IReceiveRepository : IRepository<Receive>
    {
        ReceiveDto GetReceiveDtoByID(int receiveIDSys);
    }
}
