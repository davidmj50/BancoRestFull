
using Microsoft.AspNetCore.Identity;

namespace Identity.models
{
    public class ApplicationUser : IdentityUser
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
