using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Entity;

namespace Fuji.Entity.StockManagement
{
    [Table("CheckStock")]
    public class CheckStockHead : BaseEntity
    {
        [Key]
        public int CheckStockID { get; set; }
        public DateTime CheckStockDate { get; set; }
        public int SystemQTY { get; set; }
        public int ActualQTY { get; set; }

        [NotMapped]
        public int VarianceQTY
        {
            get
            {
                return Math.Abs(this.SystemQTY - this.ActualQTY);
            }
        }

        public string YearNumber
        {
            get
            {
                return this.CheckStockDate.ToString("yyyy");
            }
        }
    }
}
