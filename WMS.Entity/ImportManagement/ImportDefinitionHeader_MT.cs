using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace WMS.Entity.ImportManagement
{
    [Table("ImportDefinitionHeader_MT")]
    public class ImportDefinitionHeader_MT : BaseEntity
    {

        public ImportDefinitionHeader_MT()
        {
            this.ImportDefinitionDetail_MT = new HashSet<ImportDefinitionDetail_MT>();
        }

        [Key]
        public int ImportDefHeadIDSys { get; set; }
        public int ProjectIDSys { get; set; }
        public string ForTable { get; set; }
        public string FormatName { get; set; }
        public string Delimiter { get; set; }
        public string Qualifier { get; set; }
        public Nullable<int> MaxHeading { get; set; }
        public string Encoding { get; set; }
        public Nullable<int> StartRow { get; set; }


        public virtual ICollection<ImportDefinitionDetail_MT> ImportDefinitionDetail_MT { get; set; }
        public List<ImportDefinitionDetail_MT> detail;
    }
}
