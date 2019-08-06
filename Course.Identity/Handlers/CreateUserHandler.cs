using System;
using System.Threading.Tasks;
using Course.Common.Commands;
using Course.Common.Events;
using Course.Common.Exceptions;
using Course.Common.RabbitMq;
using Course.Identity.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Course.Identity.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly ILogger _logger;
        private readonly IUserService _userService;
        private readonly IModel _model;

        public CreateUserHandler(
            IUserService userService,
            IModel model,
            ILogger<CreateUser> logger)
        {
            _userService = userService;
            _logger = logger;
            _model = model;
        }

        public async Task HandleAsync(CreateUser command)
        {
            _logger.LogInformation($"Creating user: '{command.Email}' with name: '{command.Name}'.");
            try 
            {
                await _userService.RegisterAsync(command.Email, command.Password, command.Name);
                var userCreated = new UserCreated(command.Email, command.Name);
                _model.BasicPublish(exchange: "",
                    routingKey: nameof(UserCreated),
                    basicProperties: null,
                    body: userCreated.ObjectToByteArray());
                _logger.LogInformation($"User: '{command.Email}' was created with name: '{command.Name}'.");

                return;
            }
            catch (CourseException ex)
            {
                _logger.LogError(ex, ex.Message);
                var createUserRejected = new CreateUserRejected(command.Email,
                    ex.Message, ex.Code);
                _model.BasicPublish(exchange: "",
                    routingKey: nameof(CreateUserRejected),
                    basicProperties: null,
                    body: createUserRejected.ObjectToByteArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var createUserRejected = new CreateUserRejected(command.Email,
                    ex.Message, "error");
                _model.BasicPublish(exchange: "",
                    routingKey: nameof(CreateUserRejected),
                    basicProperties: null,
                    body: createUserRejected.ObjectToByteArray());
            }
        }
    }
}