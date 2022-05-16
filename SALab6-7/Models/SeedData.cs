using Microsoft.EntityFrameworkCore;
using PCO.Data;
using PCO.Models.PlaceModels;

namespace PCO.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.Places.Any())
                {
                    return;   // DB has been seeded
                }
                string Square = "Square";
                string museum = "Museum";
                context.Places.AddRange(
                    new Place
                    {
                        Name = "Maidan Nezalezhnosti",
                        Category = Square,
                        Country = "Ukraine",
                        City = "Kiev",
                    },

                    new Place
                    {
                        Name = "St. Sophia's Cathedral",
                        Category = museum,
                        Country = "Ukraine",
                        City = "Kiev",
                    },

                    new Place
                    {
                        Name = "Classic Remise Dusseldorf",
                        Category = museum,
                        Country = "Germany",
                        City = "Dusseldorf",
                    },

                    new Place
                    {
                        Name = "Tower of Pisa",
                        Category = Square,
                        Country = "Italy",
                        City = "Pisa",
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

