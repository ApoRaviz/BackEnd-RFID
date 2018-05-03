using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Delivery
{
    public class Delivery
    {
        public Service Service { get; set; }
        public bool IsFixDelivery { get; set; }
        public virtual ICollection<DeliveryCompanyDelivery> DeliveryCompany { get; set; }
    }
}
