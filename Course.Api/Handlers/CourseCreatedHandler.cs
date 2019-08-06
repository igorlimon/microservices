using Course.Api.Repositories;
using Course.Common.Events;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Course.Api.Handlers
{
    public class CourseCreatedHandler : IEventHandler<CourseCreated>
    {
        private readonly ICourseRepository _repository;
        private readonly ILogger<CoursePublished> _logger;

        public CourseCreatedHandler(
            ICourseRepository repository,
            ILogger<CoursePublished> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(CourseCreated course)
        {
            using (_logger.BeginScope($"Create course {course.Id}"))
            {
                await _repository.AddAsync(new Course.Api.Models.Course
                {
                    Id = course.Id,
                    UserId = course.UserId,
                    Name = course.Name,
                    CreatedAt = course.CreatedAt,
                    Description = course.Description,
                    Date = course.Date,
                    Location = course.Location
                });
                _logger.LogInformation($"Course created: {course.Name}");
            }
        }
    }
}