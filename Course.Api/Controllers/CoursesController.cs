using System;
using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.RabbitMq;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Course.Api.Controllers
{
    [Route("[controller]")]
    public class CoursesController : Controller
    {
        private readonly IModel _model;

        public CoursesController(
            IModel model
            )
        {
            _model = model;
        }

        [HttpPost("Publish")]
        public async Task<IActionResult> Publish(Guid courseId)
        {
            var coursePublished = new CoursePublished(courseId, Guid.NewGuid());
            byte[] body = coursePublished.ObjectToByteArray();
            _model.BasicPublish(exchange: "",
                routingKey: Extensions.GetQueueName<CoursePublished>(),
                basicProperties: null,
                body: body);

            return Accepted($"courses/{courseId}");
        }
    }
}
