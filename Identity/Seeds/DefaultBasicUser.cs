using Application.Enums;
using Identity.models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Seeds
{
    public static class DefaultBasicUser
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default admin user
            var defaultUser = new ApplicationUser
            {
                UserName = "userBasic",
                Email = "useradmin@gmail.com",
                Nombre = "Pedro",
                Apellido = "Vasquez",
                EmailConfirmed = false,
                PhoneNumberConfirmed = false,
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    var result = await userManager.CreateAsync(defaultUser, "admin123");
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    //await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }
    }
}
