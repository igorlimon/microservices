using Course.Api.Repositories;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.RabbitMq;
using RabbitMQ.Client;

namespace Course.Api.Handlers
{
    public class PublishCourseHandler : ICommandHandler<PublishCourse>
    {
        private readonly ICourseRepository _repository;
        private readonly ILogger<PublishCourse> _logger;
        private readonly IModel _channel;

        public PublishCourseHandler(
            ICourseRepository repository,
            IModel channel,
            ILogger<PublishCourse> logger)
        {
            _repository = repository;
            _channel = channel;
            _logger = logger;
        }

        public async Task HandleAsync(PublishCourse course)
        {
            using (_logger.BeginScope($"Publish course {course.CourseId}"))
            {
                await _repository.PublishCourse(course.CourseId);
                var publishedCourse = new CoursePublished(course.CourseId, course.UserId);
                _channel.BasicPublish(exchange: "",
                    routingKey: nameof(CoursePublished),
                    basicProperties: null,
                    body: publishedCourse.ObjectToByteArray());
                _logger.LogInformation($"Course published: {course.CourseId}");
            }
        }
    }
}