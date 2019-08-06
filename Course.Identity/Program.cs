using Course.Common.Events;
using Course.Common.Services;

namespace Course.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceHost.Create<Startup>(args)
                .UseRabbitMq()
                //.SubscribeToEvent<UserRegistredToCourse>()
                .Build()
                .Run();
        }
    }
}
