using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace SportsStore.Models
{
    internal class IdentitySeedData
    {
        private const string AdminUser = "Admin";
        private const string AdminPassword = "Secret123$";

        internal static async void EnsurePopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppIdentityDbContext>();
            
            if ((await context.Database.GetPendingMigrationsAsync()).Any()) 
            {
                await context.Database.MigrateAsync();
            }

            UserManager<IdentityUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(AdminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin")
                {
                    Email = "admin@example.com", 
                    PhoneNumber = "555-1234"
                };
                
                await userManager.CreateAsync(user, AdminPassword);
            }
        }
    }
}