using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Entity.Employee
{
    [Table("Positions")]
    public class Positions : BaseEntity
    {
        [Key]
        public int PositionIDSys { get; set; }
        public string Acronym { get; set; }
        public string PositionName { get; set; }
        public string PositionNameEn { get; set; }
        public string PositionDescription { get; set; }
        public string Config { get; set; }
        public Nullable<int> ParentIDSys { get; set; }

        [NotMapped]
        public List<PositionConfig<List<PositionConfig<string>>>> PositionsConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<List<PositionConfig<List<PositionConfig<string>>>>>(StringHelper.Decompress(Config));
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
