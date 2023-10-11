using Application.DTOs.Users;
using Application.Features.Authenticate.Commands.AuthenticateCommand;
using Application.Features.Authenticate.Commands.RegisterCommand;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers.V1;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await Mediator.Send(new AuthenticateCommand
            {
                Email = request.Email,
                Password = request.Password,
                IpAddress = GenerateIpAddress()
            }));
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            return Ok(await Mediator.Send(new RegisterCommand
            {
                Email = request.Email,
                Password = request.Password,
                Username = request.Username,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                ConfirmPassword = request.ConfirmPassword,
                Origin = Request.Headers["origin"]
            }));
        }

        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-forwarded-For"))
            {
                return Request.Headers["X-forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
