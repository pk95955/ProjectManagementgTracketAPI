
using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManagementTracketAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ProjectManagementTracketAPI.DbContexts;
using MemberAssigendTask.Model;
using MemberAssigendTask.Cosumer;
using RabbitMQ.Client.Events;

namespace ProjectManagementTracketAPI.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _db;
      
        public MemberRepository(ApplicationDbContext db)
        {
            _db = db;
         
        }
         
        public ResponseDTO AssigningTask()
        {
            ResponseDTO response = new ResponseDTO()
            {
                IsSuccess  =false ,
            };
            try
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
                    _db.AssigningTask.Add(assigningTask);
                    _db.SaveChanges();

                };
                channel.BasicConsume("assinged-task-queue", true, consumer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;

        }
        public async Task<AssigningTaskDTO> GetAssigedTask(int MemberId)
        {
            AssigningTask assigningTask = new AssigningTask();
            assigningTask =  await _db.AssigningTask.FirstOrDefaultAsync(r => r.MemberId == MemberId);
            AssigningTaskDTO assigningTaskDTO = new AssigningTaskDTO()
            {
                Id = assigningTask.Id,
                MemberId = assigningTask.MemberId,
                MemberName = assigningTask.MemberName,
                Deliverbles = assigningTask.Deliverbles,
                TaskStartDate = assigningTask.TaskStartDate,
                TaskEndDate = assigningTask.TaskEndDate

            };
            return assigningTaskDTO;
            
        }
      

        
    }
}
