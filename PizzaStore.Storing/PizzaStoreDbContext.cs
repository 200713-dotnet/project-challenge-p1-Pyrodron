using Microsoft.EntityFrameworkCore;
using PizzaStore.Domain.Models;

namespace PizzaStore.Storing {
  public class PizzaStoreDbContext : DbContext {
    public DbSet<CrustModel> Crust { get; set; }
    public DbSet<MenuModel> Menu { get; set; }
    public DbSet<OrderModel> Orders { get; set; }
    public DbSet<PizzaModel> Pizzas { get; set; }
    public DbSet<StoreModel> Stores { get; set; }
    public DbSet<ToppingModel> Toppings { get; set; }
    public DbSet<UserModel> Users { get; set; }
    
    public PizzaStoreDbContext(DbContextOptions options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder builder) {
      builder.Entity<CrustModel>().HasKey(e => e.ID);
      builder.Entity<MenuModel>().HasKey(e => e.ID);
      builder.Entity<OrderModel>().HasKey(e => e.ID);
      builder.Entity<OrderModel>().Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
      builder.Entity<PizzaModel>().HasKey(e => e.ID);
      builder.Entity<PizzaModel>().Property(e => e.Cost).HasColumnType("decimal(18, 2)");
      builder.Entity<StoreModel>().HasKey(e => e.ID);
      builder.Entity<ToppingModel>().HasKey(e => e.ID);
      builder.Entity<UserModel>().HasKey(e => e.ID);
    }
  }
}