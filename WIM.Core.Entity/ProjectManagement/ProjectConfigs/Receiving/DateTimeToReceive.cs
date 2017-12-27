using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Receiving
{
    public class DateTimeToReceive
    {
        public ReceivingDurationTime Monday { get; set; }
        public ReceivingDurationTime Tuesday { get; set; }
        public ReceivingDurationTime Wednesday { get; set; }
        public ReceivingDurationTime Thursday { get; set; }
        public ReceivingDurationTime Friday { get; set; }
        public ReceivingDurationTime Saturday { get; set; }
        public ReceivingDurationTime Sunday { get; set; }
    }
}
