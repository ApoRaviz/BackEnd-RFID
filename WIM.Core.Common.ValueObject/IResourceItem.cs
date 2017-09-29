using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Common.ValueObject
{
    public interface IResourceItem
    {
        string Key { get; set; }
        string Message { get; set; }
        IList<string> Params { get; set; }
    }
}
