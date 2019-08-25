namespace ChushkaApp.Services
{
    using ChushkaApp.Data;
    using ChushkaApp.Models;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Linq;
    public class Seeder
    {
        public static void SeedRoles(RoleManager<IdentityRole> roleManager, ChushkaDBContext context)
        {
            if (!context.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole(roleName: "User")).Wait();
                roleManager.CreateAsync(new IdentityRole(roleName: "Admin")).Wait();
            }
        }

        public static void SeedProducts(ChushkaDBContext context)
        {
            if (!context.Products.Any())
            {
                List<Product> products = new List<Product>();
                for (int i = 0; i < 5; i++)
                {
                    var product = new Product
                    {
                        Name = $"SeededProduct_{i}",
                        Description = i%2==0?"Small description": "Each package is licensed to you by its owner. NuGet is not responsible for, nor does it grant any licenses to, third-party packages. Some packages may include dependencies which are governed by additional licenses. Follow the package source (feed) URL to determine any dependencies.",
                        Price = i * 0.35m + 1.17m,
                        Type = (ProductType)((i + 1) % 6)
                    };
                    products.Add(product);
                }
                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }


    }
}