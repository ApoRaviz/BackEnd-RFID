using HashidsNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.Utility.Attributes;
using WIM.Core.Common.Utility.UtilityHelpers;

namespace WIM.Core.Entity.LabelManagement
{
    [Table("HeadReportControls")]
    public class HeadReportControl : BaseEntity
    {
        [Key]
        public int HeadReportIDSys { get; set; }
        public int SubModuleIDSys { get; set; }
        public string ReportName { get; set; }
        public string Config { get; private set; }

        [NotMapped]
        public IEnumerable<string> HeadReportConfig
        {
            get
            {
                if (!string.IsNullOrEmpty(Config))
                {
                    return JsonConvert.DeserializeObject<List<string>>(StringHelper.Decompress(Config));
                }
                return null;
            }
            set
            {
                SetConfig(value);
            }
        }

        List<Label> headReportLabels;
        [NotMapped]
        public List<Label> HeadReportLabels
        {
            get
            {
                if (HeadReportConfig != null && HeadReportConfig.Any())
                {
                    headReportLabels = new List<Label>();
                    foreach (var item in HeadReportConfig)
                    {
                        string hashValue = HashidsHelper.EncodeHex(item);
                        headReportLabels.Add(new Label(hashValue, hashValue));
                    }
                    return headReportLabels;
                }
                return null;
            }
            private set
            {
                headReportLabels = value;
            }
        }

        private void SetConfig(IEnumerable<string> values)
        {
            Config = StringHelper.Compress(JsonConvert.SerializeObject(values));
        }        
    }
}
