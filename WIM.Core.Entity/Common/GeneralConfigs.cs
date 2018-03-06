using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.UtilityHelpers;
using Newtonsoft.Json;

namespace WIM.Core.Entity.Common
{
    [Table("GeneralConfigs")]
    public class GeneralConfigs :BaseEntity
    {
        [Key]
        public string Keyword { get; set; }
        public string Config { get; set; }

        [NotMapped]
        public DetailConfig DetailConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<DetailConfig>(StringHelper.Decompress(Config));
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
