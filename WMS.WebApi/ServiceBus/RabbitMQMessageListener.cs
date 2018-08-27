using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.MessagePatterns;
using RawRabbit.Configuration;
using RawRabbit.vNext;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using WMS.WebApi.ServiceBus.IntegrationEvents;

namespace WMS.WebApi.ServiceBus
{
    public static class RabbitMQMessageListener
    {
        private static IConnection _connection;
        private static IModel _channel;
        private static BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private static IBasicProperties props;

        private static string _ExchangeName = "wim_event_topic";

        private static string _ReceiveQueue = ConfigurationManager.AppSettings["rb:SubscriptionClientName"];
        private static string _SenderQueue = "publisher_queue";

        public static bool Start(IList<string> routingNames, IList<string> rawRoutingNames)
        {
            if (!Convert.ToBoolean(ConfigurationManager.AppSettings["rb:RabbitEnable"]))
                return false;

            var factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["rb:HostName"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(15),

            };

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["rb:Port"]))
            {
                factory.Port = Convert.ToInt32(ConfigurationManager.AppSettings["rb:Port"]);
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["rb:UserName"]))
            {
                factory.UserName = ConfigurationManager.AppSettings["rb:UserName"];
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["rb:Password"]))
            {
                factory.Password = ConfigurationManager.AppSettings["rb:Password"];
            }

            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["rb:VirsualHost"]))
            {
                factory.VirtualHost = ConfigurationManager.AppSettings["rb:VirsualHost"];
            }

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.BasicQos(0, 1, false);

            _channel.ExchangeDeclare(exchange: _ExchangeName,
                durable: true, type: ExchangeType.Topic);




            var consumer = new EventingBasicConsumer(_channel);
            GenerateSenderQueue(_ReceiveQueue, consumer, routingNames);
            consumer.Received += SubscribeSender;


            // publisher
            var consumerPub = new EventingBasicConsumer(_channel);
            GenerateConsumerRouting(_SenderQueue, routingNames, consumerPub);
            consumerPub.Received += SubscribePublisher;

            GenerateRawrabbitConsumer(rawRoutingNames);

            return true;
        }

        #region SenderConsumer
        private static void GenerateSenderQueue(string queueName, EventingBasicConsumer consumer, IList<string> routingName)
        {
            _channel.QueueDeclare(queue: queueName, durable: true,
                          exclusive: false, autoDelete: true, arguments: null);
            for (int i = 0; i < routingName.Count; i++)
            {
                _channel.QueueBind(queueName, _ExchangeName, routingName[i]+ ".#", null);
            }

            _channel.BasicConsume(queue: queueName,
            autoAck: false,
            consumer: consumer);
        }
        private static void GenerateRawrabbitConsumer(IList<string> routers)
        {
            var config = new RawRabbitConfiguration
            {
                Username = ConfigurationManager.AppSettings["rb:UserName"],
                Password = ConfigurationManager.AppSettings["rb:Password"],
                Port = Convert.ToInt32(ConfigurationManager.AppSettings["rb:Port"]),
                VirtualHost = ConfigurationManager.AppSettings["rb:VirsualHost"],
                Hostnames = { ConfigurationManager.AppSettings["rb:HostName"] }
                // more props here.
            };

            var client = BusClientFactory.CreateDefault(config);

            foreach (var route in routers)
            {
                client.RespondAsync<string, string>(async (message, context) =>
                {
                    var eventType = Type.GetType("WMS.WebApi.ServiceBus.Events." + route);
                    var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                    var handler = Type.GetType("WMS.WebApi.ServiceBus.EventsHandler." + route + "Handler");
                    object objHandler = Activator.CreateInstance(handler, new object[] { });
                    var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                    var result = concreteType.GetMethod("Handle").Invoke(objHandler, new object[] { integrationEvent });

                    if (result != null)
                        return result.ToString();

                    return string.Empty;
                }, cgf => cgf.WithExchange(w => w.WithName("wim_event_topic"))
                .WithQueue(w => w.WithName("WMSTopic")
                .WithDurability()
                .WithExclusivity(false)
                .WithAutoDelete())
                .WithRoutingKey(route));
            }
            
        }
        #endregion

        #region PublisherConsumer
        private static void GenerateConsumerRouting(string queueName, IList<string> routingName, EventingBasicConsumer consumer)
        {
            _channel.QueueDeclare(queue: queueName, durable: true,
                exclusive: false, autoDelete: true, arguments: null);

            for (int i = 0; i < routingName.Count; i++)
            {
                _channel.QueueBind(queueName, _ExchangeName, "reply_" + routingName[i], null);
            }

            _channel.BasicConsume(queue: queueName,
                autoAck: true,
                consumer: consumer);

            //var pp = _channel.CreateBasicProperties();
            //var correlationId2 = Guid.NewGuid().ToString();
            //pp.CorrelationId = correlationId2;
            //pp.ReplyTo = routingName;
            //pp.Persistent = true;

            //return pp;

        }
        #endregion

        public static void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType()
                   .Name;

            var message = JsonConvert.SerializeObject(@event);

            props = _channel.CreateBasicProperties();
            var deliveryTag = _channel.NextPublishSeqNo;
            props.DeliveryMode = 2;
            props.ReplyTo = "reply_" + eventName;


            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: _ExchangeName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: props,
                body: messageBytes);


            // return respQueue.Take();
        }

        private static void SubscribePublisher(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);
            respQueue.Add(message);
        }
        private static void SubscribeSender(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body).Trim('"').Replace("\\","");

            var props = ea.BasicProperties;
            string keyObj = ea.RoutingKey.Split('.')[0] ?? "";
            var replyProps = _channel.CreateBasicProperties();
            replyProps.DeliveryMode = 2;
            //replyProps.CorrelationId = props.CorrelationId;

            if (!string.IsNullOrEmpty(keyObj))
            {
                var eventType = Type.GetType("WMS.WebApi.ServiceBus.Events." + keyObj);
                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                var handler = Type.GetType("WMS.WebApi.ServiceBus.EventsHandler." + keyObj + "Handler");
                object objHandler = Activator.CreateInstance(handler, new object[] { });
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                concreteType.GetMethod("Handle").Invoke(objHandler, new object[] { integrationEvent });

                if (props.ReplyTo != null)
                {
                    // REPLY Message
                    var responseBytes = Encoding.UTF8.GetBytes("AckNo=" + ea.DeliveryTag);
                    _channel.BasicPublish(exchange: _ExchangeName,
                                            routingKey: props.ReplyTo,
                                            mandatory: true,
                                            basicProperties: replyProps,
                                            body: responseBytes);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag,
                      multiple: false);
            }

        }

        public static void Stop()
        {
            if (_channel != null)
                _channel.Close(200, "Goodbye");
            if (_connection != null)
                _connection.Close();
        }

    }
}