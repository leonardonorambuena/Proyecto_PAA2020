using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Proyecto_PAA.Models;

namespace Proyecto_PAA.Migrations
{
    public class CategorySeeder
    {
        public static void Seeder(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.Categories.Add(new Category
                {
                    CategoryName = "Video juegos"
                });
                context.Categories.Add(new Category
                {
                    CategoryName = "Tecnología"
                });

            }
        }
    }
}