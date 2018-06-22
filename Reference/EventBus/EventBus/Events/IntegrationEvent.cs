using System;
using System.Collections.Generic;
using System.Text;

namespace YUTSYS_SERVICES.BuildingBlocks.EventBus.Events
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
