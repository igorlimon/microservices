using System;
using System.Linq;
using System.Threading.Tasks;
using Course.Api.Repositories;
using Course.Common;
using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.RabbitMq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Course.Api.Controllers
{
    [Route("[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CoursesController : Controller
    {
        private readonly IModel _model;
        private readonly ICourseRepository _repository;

        public CoursesController(IModel model,
            ICourseRepository repository)
        {
            _model = model;
            _repository = repository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            var courses = await _repository
                .BrowseAsync(Guid.Parse(User.Identity.Name));

            return Json(courses.Select(x => new { x.Id, x.Name, x.Date, x.Location, x.CreatedAt }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var course = await _repository.GetAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            if (course.UserId != Guid.Parse(User.Identity.Name))
            {
                return Unauthorized();
            }

            return Json(course);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Post(CreateCourse command)
        {
            command.Id = Guid.NewGuid();
            command.UserId = Guid.NewGuid();
            command.CreatedAt = DateTime.UtcNow;
            _model.BasicPublish(exchange: "",
                routingKey: nameof(CreateCourse),
                basicProperties: null,
                body: command.ObjectToByteArray());

            return Accepted($"courses/{command.Id}");
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
