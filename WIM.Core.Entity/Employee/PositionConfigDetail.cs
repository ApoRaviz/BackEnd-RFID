using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace WIM.Core.Entity.Employee
{
    public class PositionConfigDetail
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string DefaultValue { get; set; }
        public string Format { get; set; }
        public string IsReset { get; set; }
    }
}
