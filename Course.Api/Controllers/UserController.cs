using System;
using System.Linq;
using System.Threading.Tasks;
using Course.Api.Repositories;
using Course.Common.Commands;
using Course.Common.RabbitMq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Course.Api.Controllers
{
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : Controller
    {
        private readonly IModel _model;
        private readonly ICourseRepository _repository;

        public UserController(IModel model,
            ICourseRepository repository)
        {
            _model = model;
            _repository = repository;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]CreateUser command)
        {
            _model.BasicPublish(exchange: "",
                routingKey: nameof(CreateUser),
                basicProperties: null,
                body: command.ObjectToByteArray());

            return Accepted($"courses/{command.Email}");
        }
    }
}
