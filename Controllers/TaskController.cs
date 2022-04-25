using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using producer.Models;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace producer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
 
        [HttpPost]
        public void Post([FromBody] Tasks taskinfo)
        {
            var factory = new ConnectionFactory() { 
                //HostName = "localhost" , 
                // Port = 30724
            HostName = Environment.GetEnvironmentVariable("RABBITMQ_HOST"),
            Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBITMQ_PORT"))
            };

            // factory.Uri = new Uri("https://reqres.in/api/login");
 
            
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Console.WriteLine(connection.res);
                channel.QueueDeclare(queue: "TaskQueue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);
 
                string email = taskinfo.Email;
                string password = taskinfo.Password;
                string task = taskinfo.Task;
                var emailBody = Encoding.UTF8.GetBytes(email);
                var passwordBody = Encoding.UTF8.GetBytes(password);
                var taskBody = Encoding.UTF8.GetBytes(task);
                
 
                channel.BasicPublish(exchange: "",
                                     routingKey: "Task",
                                     basicProperties: null,
                                     body: emailBody);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Task",
                                     basicProperties: null,
                                     body: passwordBody);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Task",
                                     basicProperties: null,
                                     body: taskBody);
            }
        }
    }
}
