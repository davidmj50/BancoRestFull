﻿
namespace Application.DTOs.Users
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
