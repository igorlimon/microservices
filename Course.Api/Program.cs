using Course.Common.Commands;
using Course.Common.Services;

namespace Course.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                .SubscribeToCommand<PublishCourse>()
                .Build()
                .Run();
        }
    }
}
