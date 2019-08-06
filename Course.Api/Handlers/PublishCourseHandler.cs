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
        private readonly ILogger<PublishCourse> _logger;
        private readonly IModel _channel;

        public PublishCourseHandler(
            IModel channel,
            ILogger<PublishCourse> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task HandleAsync(PublishCourse course)
        {
            using (_logger.BeginScope($"Publish course {course.CourseId}"))
            {
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