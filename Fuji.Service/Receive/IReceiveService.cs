using Fuji.Entity.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;

namespace Fuji.Service.Receive
{
    public interface IReceiveService : IService
    {
        ImportSerialHead GetReceived(string headId);
        Task<bool> Confirm2Stock(string headId);
    }
}
