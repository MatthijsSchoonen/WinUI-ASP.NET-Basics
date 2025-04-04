using WinUI_ASP.NET_Basics.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using BCrypt.Net;

namespace WinUI_ASP.NET_Basics.Data
{
    public class AppDbContext : DbContext
    {

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Topping> Toppings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PizzaOrder> PizzaOrders { get; set; }
        public DbSet<PizzaToppings> PizzaToppings { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=;database=Pizzaria", ServerVersion.Parse("8.0.30"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Status)
                .WithMany()
                .HasForeignKey(o => o.StatusId);

            modelBuilder.Entity<PizzaOrder>()
                .HasOne(po => po.Pizza)
                .WithMany()
                .HasForeignKey(po => po.PizzaId);

            modelBuilder.Entity<PizzaOrder>()
                .HasOne(po => po.Order)
                .WithMany()
                .HasForeignKey(po => po.OrderId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<PizzaToppings>()
                .HasOne(pt => pt.Pizza)
                .WithMany()
                .HasForeignKey(pt => pt.PizzaId);

            modelBuilder.Entity<PizzaToppings>()
                .HasOne(pt => pt.Topping)
                .WithMany()
                .HasForeignKey(pt => pt.ToppingId);

            // Seed data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Employee" },
                new Role { Id = 3, Name = "User" }
            );

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Order Received" },
                new Status { Id = 2, Name = "In Kitchen" },
                new Status { Id = 3, Name = "Ready for Pickup" },
                new Status { Id = 4, Name = "Delivered" }
            );

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Admin User", Email = "admin@pizzaria.com", Password = BCrypt.Net.BCrypt.HashPassword("admin123"), RoleId = 1 },
                new User { Id = 2, Name = "Employee User", Email = "employee@pizzaria.com", Password = BCrypt.Net.BCrypt.HashPassword("employee123"), RoleId = 2 },
                new User { Id = 3, Name = "Regular User", Email = "user@pizzaria.com", Password = BCrypt.Net.BCrypt.HashPassword("user123"), RoleId = 3 }
            );

            modelBuilder.Entity<Pizza>().HasData(
                new Pizza { Id = 1, Name = "Margherita", Price = 8.99m },
                new Pizza { Id = 2, Name = "Pepperoni", Price = 9.99m },
                new Pizza { Id = 3, Name = "Hawaiian", Price = 10.99m }
            );

            modelBuilder.Entity<Topping>().HasData(
                new Topping { Id = 1, Name = "Cheese" },
                new Topping { Id = 2, Name = "Pepperoni" },
                new Topping { Id = 3, Name = "Pineapple" }
            );

            modelBuilder.Entity<Order>().HasData(
                new Order { Id = 1, UserId = 3, StatusId = 1, OrderedAt = DateTime.Now }
            );

            modelBuilder.Entity<PizzaOrder>().HasData(
                new PizzaOrder { Id = 1, PizzaId = 1, OrderId = 1 }
            );

            modelBuilder.Entity<PizzaToppings>().HasData(
                new PizzaToppings { Id = 1, PizzaId = 1, ToppingId = 1 },
                new PizzaToppings { Id = 2, PizzaId = 2, ToppingId = 2 },
                new PizzaToppings { Id = 3, PizzaId = 3, ToppingId = 3 }
            );
        }
    }
   
}
