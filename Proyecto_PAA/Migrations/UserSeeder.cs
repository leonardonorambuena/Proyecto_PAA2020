using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto_PAA.Helpers;
using Proyecto_PAA.Models;

namespace Proyecto_PAA.Migrations
{
    public class UserSeeder
    {
        public static void Seeder(ApplicationDbContext context)
        {
            if (context.Users.Any() == false)
            {
                byte[] psHash, psSalt;
                PasswordHelper.CreatePasswordHash("123456", out psHash, out psSalt);
                context.Users.Add(new User
                {
                    Email = "tomas@inacap.cl",
                    FirstName = "Tomas",
                    LastName = "Conelli",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    PasswordSalt = psSalt,
                    PasswordHash = psHash
                });
                var user = context.Users.Add(new User
                {
                    Email = "leonardo.norambuena@inacap.cl",
                    FirstName = "Leonardo",
                    LastName = "Norambuena",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    PasswordSalt = psSalt,
                    PasswordHash = psHash
                });
                context.SaveChanges();

                var roleAdmin = context.Roles.FirstOrDefault(x => x.RoleName == StringHelper.ROLE_ADMINISTRATOR);
                if (roleAdmin != null)
                    context.UserRoles.Add(new UserRole
                    {
                        UserId = user.UserId,
                        RoleId =  roleAdmin.RoleId
                    });
            }
        }

    }
}