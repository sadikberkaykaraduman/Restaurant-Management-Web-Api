using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Concrete
{
    public class RestorantDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=berkay; initial catalog=Rastorant; integrated Security=True; TrustServerCertificate=True;");
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:platefulldb.database.windows.net,1433;Initial Catalog=PlateFullDb;Persist Security Info=False;User ID=platefullsqladmin;Password=Berkay1500?;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        public RestorantDbContext() { }

        public RestorantDbContext(DbContextOptions<RestorantDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product relations
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId);
            base.OnModelCreating(modelBuilder);

            //OrderDetail relations
            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.OrderDetailProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderDetailOrderId);

            //Basket Relations
            modelBuilder.Entity<Basket>()
                .HasOne(b => b.Product)
                .WithMany(p => p.Baskets)
                .HasForeignKey(b => b.BasketProductId);
            modelBuilder.Entity<Basket>()
                .HasOne(b => b.MenuTable)
                .WithMany(u => u.Baskets)
                .HasForeignKey(b => b.BasketMenuTableId);
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<CashRegister> CashRegisters { get; set; }
        public DbSet<MenuTable> MenuTables { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
