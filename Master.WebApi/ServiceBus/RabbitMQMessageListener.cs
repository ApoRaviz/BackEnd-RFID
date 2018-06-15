using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Master.WebApi.ServiceBus
{
    public static class RabbitMQMessageListener
    {
        private static IConnection _connection;
        private static IModel _channel;
        private static BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private static IBasicProperties props;

        private static string _ExchangeName = "tracking_event_bus";
        private static string _ReceiveQueue = ConfigurationManager.AppSettings["rb:SubscriptionClientName"];
        private static string _SenderQueue = "publisher_queue";

        public static void Start(IList<string> routingNames)
        {
            var factory = new ConnectionFactory
            {
                HostName = ConfigurationManager.AppSettings["rb:HostName"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(15)
            };

            if(!string.IsNullOrEmpty(ConfigurationManager.AppSettings["rb:UserName"]))
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

            _channel.ExchangeDeclare(exchange: _ExchangeName,
                type: ExchangeType.Direct);

            var consumer = new EventingBasicConsumer(_channel);
            GenerateSenderQueue(_ReceiveQueue, consumer, routingNames);
            consumer.Received += SubscribeSender;

            // publisher
            //var consumerPub = new EventingBasicConsumer(_channel);
            //props = GenerateConsumerRouting(_SenderQueue1, _SenderQueue1, consumerPub);

            //consumerPub.Received += SubscribePublisher;
        }

        #region SenderConsumer
        private static void GenerateSenderQueue(string queueName, EventingBasicConsumer consumer, IList<string> routingName)
        {
            _channel.QueueDeclare(queue: queueName, durable: true,
                          exclusive: false, autoDelete: true, arguments: null);
            for (int i = 0; i < routingName.Count; i++)
            {
                GenerateSenderRouting(queueName, routingName[i], consumer);
            }

            _channel.BasicConsume(queue: queueName,
            autoAck: false,
            consumer: consumer);
        }
        private static void GenerateSenderRouting(string queueName, string routingName, EventingBasicConsumer consumer)
        {
            _channel.QueueBind(queueName, _ExchangeName, routingName, null);
        }

        #endregion

        #region PublisherConsumer
        private static IBasicProperties GenerateConsumerRouting(string QueueName, string routingName, EventingBasicConsumer consumer)
                {
                    _channel.QueueDeclare(queue: QueueName, durable: true,
                      exclusive: false, autoDelete: true, arguments: null);

                    _channel.QueueBind(QueueName, _ExchangeName, routingName, null);

                    _channel.BasicConsume(queue: QueueName,
                       autoAck: true,
                       consumer: consumer);

                    var pp = _channel.CreateBasicProperties();
                    var correlationId2 = Guid.NewGuid().ToString();
                    pp.CorrelationId = correlationId2;
                    pp.ReplyTo = routingName;
                    pp.Persistent = true;

                    return pp;

                }
        #endregion

        public static void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType()
                   .Name;

            var message = JsonConvert.SerializeObject(@event);

            var messageBytes = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: _ExchangeName,
                routingKey: eventName,
                basicProperties: props,
                body: messageBytes);

           // return respQueue.Take();
        }

        public static void Stop()
        {
            if(_channel != null)
                _channel.Close(200, "Goodbye");
            if(_connection != null)
            _connection.Close();
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
            var message = Encoding.UTF8.GetString(body);

            var props = ea.BasicProperties;
            string keyObj = ea.RoutingKey;
            //var replyProps = _channel.CreateBasicProperties();
            //replyProps.CorrelationId = props.CorrelationId;

            var eventType = Type.GetType("Master.WebApi.ServiceBus.Events." + keyObj);
            var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
            var handler = Type.GetType("Master.WebApi.ServiceBus.EventsHandler." + keyObj + "Handler");
            object objHandler = Activator.CreateInstance(handler, new object[] { });
            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
            concreteType.GetMethod("Handle").Invoke(objHandler, new object[] { integrationEvent });

            _channel.BasicAck(deliveryTag: ea.DeliveryTag,
              multiple: false);


            if (props.ReplyTo != null)
            {
                // REPLY Message
                //var responseBytes = Encoding.UTF8.GetBytes("Received Cammand: " + message);
                //_channel.BasicPublish(exchange: _ExchangeName, routingKey: props.ReplyTo,
                //  basicProperties: replyProps, body: responseBytes);

                //_channel.BasicAck(deliveryTag: ea.DeliveryTag,
                //  multiple: false);
            }
        }
    }
}