using Microsoft.EntityFrameworkCore;
using PizzaStore.Domain.Models;

namespace PizzaStore.Storing {
  public class PizzaStoreDbContext : DbContext {
    public DbSet<Menu> Menu { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Store> Stores { get; set; }
    public DbSet<User> Users { get; set; }
    
    public PizzaStoreDbContext(DbContextOptions options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder builder) {
      builder.Entity<Menu>().HasKey(e => e.ID);
      builder.Entity<Order>().HasKey(e => e.ID);
      builder.Entity<Order>().Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
      builder.Entity<Pizza>().HasKey(e => e.ID);
      builder.Entity<Pizza>().Property(e => e.Cost).HasColumnType("decimal(18, 2)");
      builder.Entity<Store>().HasKey(e => e.ID);
      builder.Entity<User>().HasKey(e => e.ID);
    }
  }
}