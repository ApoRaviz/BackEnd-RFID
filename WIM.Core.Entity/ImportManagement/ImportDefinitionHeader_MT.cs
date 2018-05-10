using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ImportManagement
{
    [Table("ImportDefinitionHeader_MT")]
    public class ImportDefinitionHeader_MT : BaseEntity
    {

        public ImportDefinitionHeader_MT()
        {
            this.ImportDefinitionDetail_MT = new HashSet<ImportDefinitionDetail_MT>();
        }

        [Key]
        public int ImportIDSys { get; set; }
        public string ImportID { get; set; }
        public string ForTable { get; set; }
        public string FormatName { get; set; }
        public string Delimiter { get; set; }
        public Nullable<int> MaxHeading { get; set; }
        public string Encoding { get; set; }
        public Nullable<bool> SkipFirstRecode { get; set; }
        
        public virtual ICollection<ImportDefinitionDetail_MT> ImportDefinitionDetail_MT { get; set; }
        public List<ImportDefinitionDetail_MT> detail;
    }
}
