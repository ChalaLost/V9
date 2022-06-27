using MassTransit;
using Microsoft.AspNet.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using V9AgentInfo.Hubs;
using V9AgentInfo.Models.Entities;

namespace Client.Consumer
{
    public class TopicExchangeConsumer
    {
        /*private readonly IHubContext<SignalR> _hub;
        public TopicExchangeConsumer(IHubContext<SignalR> hub)
        {
            _hub = hub;
        }
        public async Task Consume(ConsumeContext<InfoDemo> context)
        {
            await Task.Run(() => { var obj = context.Message.data; });
            List<Info> a = context.Message.data.OrderByDescending(s => s.Id).ToList();
            var obj = context.Message.data;
            await _hub.Clients.All.SendAsync("NewList", obj);
        }*/
        public void Consume(IModel channel, ConsumeContext<InfoDemo> context)
        {

            channel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic);
            channel.QueueDeclare("demo-topic-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("demo-topic-queue", "demo-topic-exchange", "account.*");
            channel.BasicQos(0, 10, false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };

            channel.BasicConsume("demo-topic-queue", true, consumer);
        }
    }
}
