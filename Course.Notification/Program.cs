using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.Services;

namespace Course.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .SubscribeToEvent<CoursePublished>()
                .SubscribeToCommand<SendFeedbackForm>()
                .Build()
                .Run();
        }
    }
}
