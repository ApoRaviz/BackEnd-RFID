using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Entity.LayoutManagement
{
    [Table("LabelLayoutHeader_MT")]
    public class LabelLayoutHeader_MT
    {        
        public LabelLayoutHeader_MT()
        {
            this.LabelLayoutDetail_MT = new HashSet<LabelLayoutDetail_MT>();
        }

        [Key]
        public int LabelIDSys { get; set; }
        public string LabelID { get; set; }
        public string FormatName { get; set; }
        public Nullable<decimal> Width { get; set; }
        public string WidthUnit { get; set; }
        public Nullable<decimal> Height { get; set; }
        public string HeightUnit { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UserUpdate { get; set; }
        public string ForTable { get; set; }
        
        public virtual ICollection<LabelLayoutDetail_MT> LabelLayoutDetail_MT { get; set; }
        public List<LabelLayoutDetail_MT> detail;
    }
}
