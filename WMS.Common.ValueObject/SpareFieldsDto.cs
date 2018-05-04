using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Common.ValueObject
{
    public class SpareFieldsDto
    {
        int SpfIDSys { get; set; }
        int ProjectIDSys { get; set; }
        int SpfdIDSys { get; set; }
        int SpfRefID { get; set; }
        string Text { get; set; }
        string Type { get; set; }
        string TableName { get; set; }
        string Value { get; set; }
    }
}
