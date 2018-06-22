using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace YUTSYS_SERVICES.BuildingBlocks.EventBusRabbitMQ
{
    public interface IRabbitMQPersistentConnection
         : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
