using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.ControlMaster
{
    [Table("Control_MT")]
    public class Control_MT : BaseEntity
    {
        [Key]
        public int ControlIDSys { get; set; }
        public string ControlDetail { get; set; }

        [NotMapped]
        public List<ControlValue> ControlDetails
        {
            get
            {
                if (!string.IsNullOrEmpty(ControlDetail))
                {
                    return JsonConvert.DeserializeObject<List<ControlValue>>(StringHelper.Decompress(ControlDetail));
                }
                return null;
            }
            set
            {
                ControlDetail = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }
    }
}
