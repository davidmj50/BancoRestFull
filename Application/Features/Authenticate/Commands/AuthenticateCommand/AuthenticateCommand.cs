using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authenticate.Commands.AuthenticateCommand
{
    public class AuthenticateCommand : IRequest<Response<AuthenticationResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string IpAddress { get; set; }
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, Response<AuthenticationResponse>>
    {
        private readonly IAccountServices _accountServices;
        public AuthenticateCommandHandler(IAccountServices accountServices) 
        {
            _accountServices = accountServices;
        }

        public async Task<Response<AuthenticationResponse>> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            return await _accountServices.AuthenticateAsync(new AuthenticationRequest
            {
                Email = request.Email,
                Password = request.Password
            },  request.IpAddress);
        }
    }
}
