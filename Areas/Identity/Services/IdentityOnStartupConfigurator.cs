using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using URLShortener.Areas.Identity.Data;
using URLShortener.Areas.Identity.Services.Configurations;

namespace URLShortener.Areas.Identity.Services
{
    public static class IdentityOnStartupConfigurator
    {
        //private readonly IServiceProvider _serviceProvider;
        //private readonly IOptions<IdentityConfiguration> _options;

        //public IdentityOnStartupConfigurator(IServiceProvider serviceProvider, IOptions<IdentityConfiguration> options)
        //{
        //    _serviceProvider = serviceProvider;
        //    _options = options;

        //}

        public static async Task CreateRoles(IServiceProvider serviceProvider, IOptions<IdentityConfiguration> options)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = options.Value.Roles.ToList();

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task CreateAdmin(IServiceProvider serviceProvider, IOptions<IdentityConfiguration> options)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var adminName = options.Value.AdminEmail;
            var adminPassword = options.Value.AdminPassword;

            var user = await userManager.FindByEmailAsync(adminName);

            // check if the user exists
            if (user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var adminUser = new User
                {
                    UserName = adminName,
                    Email = adminName,
                    EmailConfirmed = true
                };

                var createPowerUser = await userManager.CreateAsync(adminUser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                }
            }
        }
    }

    
}
