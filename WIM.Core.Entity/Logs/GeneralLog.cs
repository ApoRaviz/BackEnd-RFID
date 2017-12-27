using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Extentions;

namespace WIM.Core.Entity.Logs
{
    [Table("GeneralLogs")]
    public class GeneralLog : BaseEntity
    {

        public GeneralLog(string propName, object obj, string createAndUpdateBy, string remark = "")
        {
            Type typeObj = obj.GetType();

            List<string> namePropKeys = typeObj.GetPropertiesName<KeyAttribute>(obj);
            string[] keys = new string[namePropKeys.Count];
            for (int i = 0; i < namePropKeys.Count; i++)
            {
                keys[i] = typeObj.GetProperty(namePropKeys[i]).GetValue(obj, null) + "";
            }

            TableName = typeObj.GetAttributeValue((TableAttribute dna) => dna.Name);
            ColumnName = propName;
            RefID = string.Join(",", keys);
            Value = typeObj.GetProperty(propName).GetValue(obj, null) + "";
            Remark = remark;
            CreateBy = UpdateBy = createAndUpdateBy;
            CreateAt = UpdateAt = DateTime.Now;
            IsActive = true;
        }

        [Key]
        public int LogIDSys { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string RefID { get; set; }
        public string Value { get; set; }
        public string Remark { get; set; }

    }
}
