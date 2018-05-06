
namespace WMS.Common.ValueObject
{
    public class SpareFieldsDto
    {
        public int SpfIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public int? SpfdIDSys { get; set; }
        public int? SpfdRefID { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public string Value { get; set; }
    }
}
