using Course.Api.Handlers;
using Course.Api.Repositories;
using Course.Common.Auth;
using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.Mongo;
using Course.Common.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Course.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddJwt(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddMongoDB(Configuration);
            services.AddSingleton<IEventHandler<CourseCreated>, CourseCreatedHandler>();
            services.AddSingleton<ICommandHandler<PublishCourse>, PublishCourseHandler>();
            services.AddSingleton<ICourseRepository, CourseRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
