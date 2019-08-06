using Course.Common.Commands;
using Course.Common.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace Course.Common.RabbitMq
{
    public static class Extensions
    {
        public static void WithCommandHandlerAsync<TCommand>(this IModel channel,
            ICommandHandler<TCommand> handler) where TCommand : ICommand
        {
            channel.QueueDeclare(queue: GetQueueName<TCommand>(),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body;
                var command = (TCommand) body.ByteArrayToObject();
                handler.HandleAsync(command);
            };
            channel.BasicConsume(queue: GetQueueName<TCommand>(),
                autoAck: true,
                consumer: consumer);
        }

        public static void WithEventHandlerAsync<TEvent>(this IModel channel,
            IEventHandler<TEvent> handler) where TEvent : IEvent
        {
            channel.QueueDeclare(queue: GetQueueName<TEvent>(),
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var @event = (TEvent)body.ByteArrayToObject();
                handler.HandleAsync(@event);
            };
            channel.BasicConsume(queue: GetQueueName<TEvent>(),
                autoAck: true,
                consumer: consumer);
        }

        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            var factory = new ConnectionFactory();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(factory);
            services.AddSingleton<IConnectionFactory>(_ => factory);
            IConnection connection = factory.CreateConnection();
            services.AddSingleton<IConnection>(_ => connection);
            IModel channel = connection.CreateModel();
            services.AddSingleton<IModel>(_ => channel);
        }

        public static string GetQueueName<T>()
            => $"{Constants.QueueRootName}/{typeof(T).Name}";

        public static byte[] ObjectToByteArray(this Object obj)
        {
            if (obj == null)
                return null;
            var bf = new BinaryFormatter();
            var ms = new MemoryStream();
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }

        public static Object ByteArrayToObject(this byte[] arrBytes)
        {
            var memStream = new MemoryStream();
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
    }
}