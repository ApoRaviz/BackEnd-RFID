using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIM.Core.Common.ValueObject;
using Master.Master;

namespace Master.Common.ValueObject
{
    public class ItemDto
    {
        public ItemDto()
        {
            this.ItemUnitMapping = new HashSet<ItemUnitDto>();
        }

        public int ItemIDSys { get; set; }        
        public string ItemCode { get; set; }
        public string JAN { get; set; }
        public string ScanCode { get; set; }
        public string ItemName { get; set; }
        public string ItemColor { get; set; }
        public string Description { get; set; }
        public string ExpControl { get; set; }
        public string SerialControl { get; set; }
        public string SerialDigit { get; set; }
        public string SerialFormat { get; set; }
        public string SupID { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string Spare4 { get; set; }
        public string Spare5 { get; set; }
        public string InspectCont { get; set; }
        public string ExpireCont { get; set; }
        public string DimensionCont { get; set; }
        public string BoxCont { get; set; }
        public string LotCont { get; set; }
        public string PalletCont { get; set; }
        public string ItemSetCont { get; set; }
        public string MiniAlert { get; set; }
        public string AlertExp { get; set; }
        public string TaxCond { get; set; }
        public byte TaxPerc { get; set; }
        public byte Active { get; set; }

        public int ProjectIDSys { get; set; }
        public ProjectDto Project_MT { get; set; }

        public ICollection<ItemUnitDto> ItemUnitMapping { get; set; }
    }
}
