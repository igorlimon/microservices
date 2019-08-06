using System.Threading.Tasks;
using Course.Common.Events;
using Course.Common.RabbitMq;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Course.Notification.Handlers
{
    public class CoursePublishedHandler : IEventHandler<CoursePublished>
    {
        private readonly IModel _channel;
        private readonly ILogger<CoursePublished> _logger;

        public CoursePublishedHandler(
            IModel channel,
            ILogger<CoursePublished> logger)
        {
            _channel = channel;
            _logger = logger;
        }

        public async Task HandleAsync(CoursePublished course)
        {
            using (_logger.BeginScope($"Registration notification {course.CourseId}"))
            {
                _logger.LogInformation($"Registration for course: {course.CourseId}");
            }
            //var publishedCourse = new CoursePublished(course.CourseId, course.UserId);
            //_channel.BasicPublish(exchange: "",
            //    routingKey: nameof(CoursePublished),
            //    basicProperties: null,
            //    body: publishedCourse.ObjectToByteArray());
        }
    }
}