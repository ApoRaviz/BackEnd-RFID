using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.Common
{
    [Table("GeneralConfigs")]
    public class BaseGeneralConfig : BaseEntity
    {
        [Key]
        public string Keyword { get; set; }
        public string Config { get; set; }
    }
}
