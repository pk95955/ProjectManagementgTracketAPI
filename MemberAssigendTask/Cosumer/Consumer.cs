using MemberAssigendTask.Model;
using Newtonsoft.Json;
using ProjectManagementTracketAPI.DbContexts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace MemberAssigendTask.Cosumer
{
    public class Consumer
    {
        public void ConsumeMeggae(ApplicationDbContext db)
        {
            // published message rabbit mq
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://guest:guest@localhost:5672")

            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare("assinged-task-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            var consumer = new EventingBasicConsumer(channel);
            var message = "";
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                message = Encoding.UTF8.GetString(body);
                AssigningTask assigningTask = new AssigningTask();
                assigningTask = JsonConvert.DeserializeObject<AssigningTask>(message);
                assigningTask.Id = 0;
                AssignedTAsk(db, assigningTask);
            };
            channel.BasicConsume("assinged-task-queue", true, consumer);

        }
        public async void AssignedTAsk(ApplicationDbContext _db, AssigningTask assigningTask)
        {

            _db.AssigningTask.Add(assigningTask);
            await _db.SaveChangesAsync();

        }

    }

}
