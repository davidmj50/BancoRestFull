using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Helpers;
using Identity.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _identityRole;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<JWTSettings> _jwtSettings;
        private readonly IDateTimeService _dateTimeService;

        public AccountServices(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> identityRole, 
            SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwtSettings, IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _identityRole = identityRole;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _dateTimeService = dateTimeService;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
            {
                throw new ApiException($"No hay una cuenta registrada con el email {request.Email}");
            }
            var result = await _signInManager.PasswordSignInAsync(usuario.UserName!, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded) 
            {
                throw new ApiException($"Las credenciales del usuario no son validas ${request.Email}.");
            }

            JwtSecurityToken token = await GenerateJWTToken(usuario);
            AuthenticationResponse response = new AuthenticationResponse();
            response.Id = usuario.Id;
            response.JwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            response.Email = usuario.Email;
            response.Username = usuario.UserName;


            var roleList = await _userManager.GetRolesAsync(usuario).ConfigureAwait(false);
            response.Roles = roleList.ToList();
            response.IsVerified = usuario.EmailConfirmed;

            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, $"Usuario autenticado {usuario.UserName}");
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress,
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {

            var userClaims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }
            string ipAddress = IpHelper.GetIpAddress();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("uid", usuario.Id),
                new Claim("ip", ipAddress)
            }.Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var signingCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Value.Issuer,
                audience: _jwtSettings.Value.Audience,
                claims : claims,
                expires : DateTime.Now.AddMinutes(_jwtSettings.Value.DurationInMinutes),
                signingCredentials : signingCredential
                );
            return jwtSecurityToken;
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var usuarioConElMismoUsername = await _userManager.FindByNameAsync(request.Username);
            if (usuarioConElMismoUsername != null)
            {
                throw new ApiException($"El nombre de usuario {request.Username} ya fue registrado previamente");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.Username,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var usuarioConElMismoCorreo = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioConElMismoCorreo != null)
            {
                throw new ApiException($"El nombre de usuario {request.Username} ya fue registrado previamente");
            }
            else
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Basic.ToString());
                    return new Response<string>(user.Id, message: $"usuario registrado exitosamente! {request.Username}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
        }
    }
}
