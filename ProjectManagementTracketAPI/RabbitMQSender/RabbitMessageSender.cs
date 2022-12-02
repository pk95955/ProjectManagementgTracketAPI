using Newtonsoft.Json;
using ProjectManagementTracketAPI.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI.RabbitMQSender
{
    public class RabbitMessageSender
    {
        public void PublishMessage(string  rabbitMQConstr, string queueName, AssigningTask assigningTask)
        {
            // published message rabbit mq
            var factory = new ConnectionFactory
            {
                //Uri = new Uri("amqp://guest:guest@localhost:5672")
                Uri = new Uri(rabbitMQConstr)

            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
           // channel.QueueDeclare("assinged-task-queue",
            channel.QueueDeclare(queueName,
                durable: true, exclusive: false, autoDelete: false,

                arguments: null);
            //  var message = new { Name = "Message-assignedtask", Message = "hello world" };
            var body = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(assigningTask));
            channel.BasicPublish("", queueName, null, body);
        }
    }
}
