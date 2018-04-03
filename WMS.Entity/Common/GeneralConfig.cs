﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.Common
{
    [Table("GeneralConfigs")]
    public class GeneralConfig : BaseGeneralConfig
    {
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
