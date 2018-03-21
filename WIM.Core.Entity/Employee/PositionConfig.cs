using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.Collections.Generic;

namespace WIM.Core.Entity.Employee
{
    public class PositionConfig<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
        public string IsReset { get; set; }
    }
}
