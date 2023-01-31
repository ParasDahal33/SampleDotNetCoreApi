using BusReservationSystemApi.Data.Enumeration;
using BusReservationSystemApi.Data.Models;
using Microsoft.AspNetCore.Identity;


namespace BusReservationSystemApi.Data.SeedData
{
    public class SeedData
    {
        public async Task SeedUserData(IServiceProvider serviceProvider)
        {
            var roleManger = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Make sure that user is seeded after roles are seeded.
            await SeedRoles(roleManger);
            await SeedUser(userManager);
        }

        private async Task SeedUser(UserManager<AppUser> userManager)
        {
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();

            // Create users using user data from UserData class.
            foreach (var user in UserData.USER_LIST)
            {
                // Currently all users have the same password.
                await userManager.CreateAsync(user, "Mypassword1!");
                if (user.UserName == "Admin")
                {
                    await userManager.AddToRoleAsync(user, Enum.GetName(UserRoles.Admin));
                }
                else if (user.UserName == "User")
                {
                    await userManager.AddToRoleAsync(user, Enum.GetName(UserRoles.User));
                }
                else if (user.UserName == "Client")
                {
                    await userManager.AddToRoleAsync(user, Enum.GetName(UserRoles.Client));
                }
                else
                {
                    await userManager.AddToRoleAsync(user, Enum.GetName(UserRoles.Manager));
                }
            }
        }

        private async Task SeedRoles(RoleManager<IdentityRole> roleManger)
        {
            foreach (var role in Enum.GetNames<UserRoles>())
            {
                await roleManger.CreateAsync(new IdentityRole(role));
            }

        }

    }
}
