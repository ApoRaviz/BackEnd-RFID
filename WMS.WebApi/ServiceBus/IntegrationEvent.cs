using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.WebApi.ServiceBus
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreateAt = DateTime.UtcNow;
            CreateBy = "anonymous";
        }

        public Guid Id { get; }
        public DateTime CreateAt { get; }
        public string CreateBy { get; }
    }
}
