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
            if (context.Users.Any(x => x.Email == "tomas@inacap.cl") == false)
            {
                byte[] psHash, psSalt;
                PasswordHelper.CreatePasswordHash("secret", out psHash, out psSalt);
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
            }
        }

    }
}