using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common
{
    public interface IResourceItem
    {
        string Key { get; set; }
        string Message { get; set; }
        IList<string> Params { get; set; }
    }
}
