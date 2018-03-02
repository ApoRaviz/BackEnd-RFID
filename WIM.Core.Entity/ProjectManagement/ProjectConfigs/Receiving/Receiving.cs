using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WIM.Core.Entity.ProjectManagement.ProjectConfigs.Receiving
{
    public class Receiving
    {
        public ReceivingDataType ReceivingDataType { get; set; }
        public SendingDataType SendingDataType { get; set; }
        public DateTimeToReceive DateTimeToReceiveData { get; set; }
        public DateTimeToReceive DateTimeToReceiveGoods { get; set; }
    }
}
