using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public static class SeedManager
    {
        public static async Task Seed(IServiceProvider services)
        {
            await SeedRoles(services);

            await SeedAdminUser(services);
        }

        private static async Task SeedRoles(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            await roleManager.CreateAsync(new IdentityRole<int>(Role.ADMIN.ToString()));
            await roleManager.CreateAsync(new IdentityRole<int>(Role.USER.ToString()));
        }

        private static async Task SeedAdminUser(IServiceProvider services)
        {
            var context = services.GetRequiredService<ParkingContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();

            var query = from user in context.User
                        where user.UserName == "admin@gmail"
                        select user;

            var adminUser = await query.FirstOrDefaultAsync();

            if (adminUser is null)
            {
                adminUser = new User { UserName = "admin@gmail.com", Email = "admin@gmail.com" };
                await userManager.CreateAsync(adminUser, "VerySecretPassword!1");
                await userManager.AddToRoleAsync(adminUser, Role.ADMIN.ToString());

                var basicUser = new User { UserName = "test@gmail.com", Email = "test@gmail.com" };
                await userManager.CreateAsync(basicUser, "adMIN1234!");
                await userManager.AddToRoleAsync(adminUser, Role.USER.ToString());
            }

            
        }
    }

}
