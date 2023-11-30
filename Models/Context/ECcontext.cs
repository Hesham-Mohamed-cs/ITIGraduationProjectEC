using GraduaitionProjectITI.Models.Configration;
using GraduaitionProjectITI.Models.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace GraduaitionProjectITI.Models.Context
{
    public class ECcontext:IdentityDbContext<ApplicationUser>
    {
        //public DbSet<ApplicationUser> Users  { get; set; }
        public DbSet<Product> Products  { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Reviews> reviews { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<SubOrder> subOrders { get; set; }


        public ECcontext( DbContextOptions<ECcontext> options ):base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=GraduationProjectDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string ROLE_ID_Admin = "ad376a8f-9eab-4bb9-9fca-30b01540f445";
            const string ROLE_ID_USER = "ad376a8f-9eab-4bb9-9fca-30b01540f777";
            var role = builder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = ROLE_ID_Admin,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = ROLE_ID_USER,
                    Name = "User",
                    NormalizedName = "User"
                });
            builder.Entity<ApplicationUser>().HasData(
                 new ApplicationUser
                 {
                     Id = ADMIN_ID,
                     Email = "Ayman123@gmail.com",
                     UserName = "Ayman123@gmail.com",
                     NormalizedEmail="AYMAN123@GMAIL.COM",
                     NormalizedUserName = "AYMAN123@GMAIL.COM",
                     PasswordHash = "AQAAAAIAAYagAAAAEH2XzyoLz+knSF5ueC5ART89pb8N4CR8g25c+L8w7w2ZQzQTlw+X551f87NqrjHqNg==", // Admin@123
                     FirstName = " Ayman ",
                     LastName = "Sayed",
                     Adress = "Cairo , Egypt",
                     Phone = "01142202287"
                 });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID_Admin,
                UserId = ADMIN_ID
            });
            builder.ApplyConfiguration(new ProductConfigration());
            builder.ApplyConfiguration(new CategoryConfigration());
            builder.ApplyConfiguration(new BrandConfigration());


        }
    }
}
