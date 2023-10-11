using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    public class RegisterCommand : IRequest<Response<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string ConfirmPassword { get; set; }
        public string Origin { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<string>>
    {
        private readonly IAccountServices _accountServices;
        public RegisterCommandHandler(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }
        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _accountServices.RegisterAsync(new RegisterRequest
            {
                Email = request.Email, 
                Password = request.Password, 
                Username = request.Username, 
                Nombre = request.Nombre, 
                Apellido = request.Apellido,
            }, request.Origin);
        }
    }
}
