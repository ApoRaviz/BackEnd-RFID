using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Inventory
{
    public class Inventory
    {
        public ServiceInventory Service { get; set; }
        public bool IsFixDelivery { get; set; }
        public virtual ICollection<DeliveryCompanyInventory> DeliveryCompany { get; set; }
    }
}
