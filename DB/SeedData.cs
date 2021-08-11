using Exist.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exist.DB
{
    public static class SeedData
    {
        public static void Initialize(MyDbContext context, UserManager<User> userManager)
        {
            if (!context.Roles.Any())
            {
                var roleAdmin = new IdentityRole<int>()
                { Name = "Admin", NormalizedName = "ADMIN" };

                var roleSuperAdmin = new IdentityRole<int>()
                { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" };

                context.Roles.AddRange(roleAdmin, roleSuperAdmin);

                context.SaveChanges();

                User user = new() { UserName = "Jo", Email = "jojo@mail.com" };

                userManager.CreateAsync(user, "Password1!").Wait();

                userManager.AddToRolesAsync(user, new List<string>() { "Admin", "SuperAdmin" }).Wait();
            }
        }
    }
}
