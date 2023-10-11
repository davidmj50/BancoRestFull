using Application.DTOs.Users;
using Application.Wrappers;

namespace Application.Interfaces
{
    public interface IAccountServices
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<Response<string>> RegisterAsync(RegisterRequest request, string origin);
    }
}
