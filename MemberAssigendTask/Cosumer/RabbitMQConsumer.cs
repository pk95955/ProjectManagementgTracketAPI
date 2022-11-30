using MemberAssigendTask.Model;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ProjectManagementTracketAPI.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemberAssigendTask.Cosumer
{
    public class RabbitMQConsumer: BackgroundService
    {
        private readonly MemberRepository _memberRepository;
        private IConnection connection;
        private IModel channel;
        public RabbitMQConsumer()
        {
            //_memberRepository = memberRepository;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password ="guest"
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: "assinged-task-queue", false, false, false, arguments: null);

        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                AssigningTask assigningTask = new AssigningTask();
                assigningTask = JsonConvert.DeserializeObject<AssigningTask>(content);
                HandleMessage(assigningTask).GetAwaiter().GetResult();
                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume("assinged-task-queue", false, consumer);
            return Task.CompletedTask;
        }
        private async Task HandleMessage(AssigningTask assigningTask)
        {
            throw new NotImplementedException();
        }
    }
}
