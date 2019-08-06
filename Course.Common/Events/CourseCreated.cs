using System;

namespace Course.Common.Events
{
    public class CourseCreated : IAuthenticatedEvent
    {
        public Guid Id { get; }
        public Guid UserId { get; }
        public string Category { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
        public string Location { get; }
        public DateTime Date { get; }

        protected CourseCreated()
        {
        }

        public CourseCreated(
            Guid id, 
            Guid userId,
            string category, 
            string name, 
            string description, 
            DateTime createdAt,
            DateTime date,
            string location)
        {
            Id = id;
            UserId = userId;
            Category = category;
            Name = name;
            Description = description;
            CreatedAt = createdAt;
            Date = date;
            Location = location;
        }
    }
}