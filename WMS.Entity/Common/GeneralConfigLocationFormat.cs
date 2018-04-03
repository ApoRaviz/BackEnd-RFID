using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Entity;

namespace WMS.Entity.Common
{
    [NotMapped]
    public partial class GeneralConfigLocationFormat  : BaseGeneralConfig
    {
        public List<LocationFormat> DetailConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<List<LocationFormat>>(StringHelper.Decompress(Config));
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
