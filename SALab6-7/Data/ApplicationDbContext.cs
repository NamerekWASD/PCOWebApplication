using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PCO.Models.PlaceModels;
using PCO.Models.UserModels;

namespace PCO.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>
    {
        public ApplicationDbContext()
            : base() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Place>? Places { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<FileModel>? Files { get; set; }
        public DbSet<RequestStore>? Requests { get; set; }
        public DbSet<RequestedPlace>? RequestedPlaces { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PCOdb;Trusted_Connection=True;");
        }
    }
}