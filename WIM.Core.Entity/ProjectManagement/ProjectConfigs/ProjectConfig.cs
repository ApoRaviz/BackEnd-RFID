using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs
{
    public class ProjectConfig
    {
        public Main.Main Main { get; set; }
        public Receiving.Receiving Receiving { get; set; }
        public Inventory.Inventory Inventory { get; set; }
        public Order.Order Order { get; set; }
        public Inspection.Inspection Inspection { get; set; }
        public Delivery.Delivery Delivery { get; set; }
        public IEnumerable<SpareField.SpareField> SpareField { get; set; }
    }
}
