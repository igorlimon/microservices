using System;

namespace Course.Common.Events
{
    public class CreateCourseRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }

        protected CreateCourseRejected()
        {
        }

        public CreateCourseRejected(Guid id, 
            string reason, string code)
        {
            Id = id;
            Reason = reason;
            Code = code;
        }
    }
}