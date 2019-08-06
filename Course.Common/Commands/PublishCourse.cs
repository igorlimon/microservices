using System;

namespace Course.Common.Commands
{
    [Serializable()]
    public class PublishCourse : IAuthenticatedCommand
    {
        public Guid CourseId { get; set; }
        public Guid UserId { get; set; }

        protected PublishCourse()
        {
        }

        public PublishCourse(Guid courseId, Guid userId)
        {
            CourseId = courseId;
            UserId = userId;
        }
    }
}