using BedeSlots.Data;
using BedeSlots.Data.Models;
using BedeSlots.Services.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BedeSlots.Web.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<BedeSlotsDbContext>().Database.Migrate();

                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var roles = new[]
                        {
                            WebConstants.MasterAdminRole,
                            WebConstants.AdminRole,
                            WebConstants.UserRole
                        };

                        foreach (var role in roles)
                        {
                            var roleExists = await roleManager.RoleExistsAsync(role);

                            if (!roleExists)
                            {
                                await roleManager.CreateAsync(new IdentityRole(role));
                            }
                        }

                        var adminEmail = WebConstants.AdminEmail;
                        var adminName = WebConstants.AdminName;
                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser == null)
                        {
                            adminUser = new User
                            {
                                Email = adminEmail,
                                UserName = adminEmail,
                                FirstName = adminName,
                                LastName = adminName,
                                Birthdate = new DateTime(1970, 01, 01),
                                Currency = ServicesConstants.BaseCurrency
                            };

                            var createAdmin = await userManager.CreateAsync(adminUser, "123456");

                            if (createAdmin.Succeeded)
                            {
                                await userManager.AddToRoleAsync(adminUser, WebConstants.MasterAdminRole);
                            }
                        }
                    })
                    .Wait();
            }
            return app;
        }
    }
}
