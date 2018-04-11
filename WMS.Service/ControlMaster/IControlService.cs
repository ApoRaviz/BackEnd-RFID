using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Service;
using WMS.Entity.ControlMaster;

namespace WMS.Service.ControlMaster
{
    public interface IControlService : IService
    {
        IEnumerable<Control_MT> GetControl();
        Control_MT GetControl(int id);
        int CreateControl(Control_MT control);
        bool UpdateControl(Control_MT control);
        bool DeleteControl(int id);
    }
}
