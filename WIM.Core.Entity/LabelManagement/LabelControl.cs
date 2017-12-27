using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Common.Utility.Helpers;
using WIM.Core.Entity.LabelManagement.LabelConfigs;

namespace WIM.Core.Entity.LabelManagement
{
    [Table("LabelControls")]
    public class LabelControl : BaseEntity
    {
        [Key]
        public int LabelIDSys { get; set; }
        public int ProjectIDSys { get; set; }        
        public string Lang { get; set; }
        [GeneralLog]
        public string Config { get; private set; }

        [NotMapped]
        public List<LabelConfig> LabelConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<List<LabelConfig>>(StringHelper.Decompress(Config));
                }
                return null;
            }
            set
            {
                Config = StringHelper.Compress(JsonConvert.SerializeObject(value));
            }
        }
    }

}
