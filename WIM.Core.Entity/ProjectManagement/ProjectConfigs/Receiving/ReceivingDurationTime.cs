using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Receiving
{
    public class ReceivingDurationTime
    {
        public bool IsActive { get; set; }
        public virtual ICollection<Duration> Duration { get; set; }
    
    }
}
