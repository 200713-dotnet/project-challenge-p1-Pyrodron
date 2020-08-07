using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;

namespace PizzaStore.Client {
    public class Program {
        static void Temp(IHost host) {
          using (var scope = host.Services.CreateScope()) {
            IServiceProvider services = scope.ServiceProvider;
            using (var context = new PizzaStoreDbContext(services.GetRequiredService<DbContextOptions<PizzaStoreDbContext>>())) {
              // context.Crust.AddRange(
              //   new CrustModel {
              //     Name = "Garlic Butter"
              //   }, new CrustModel {
              //     Name = "Stuffed"
              //   }, new CrustModel {
              //     Name = "Thick"
              //   }, new CrustModel {
              //     Name = "Thin"
              //   }, new CrustModel {
              //     Name = "All"
              //   }
              // );
              // context.Menu.AddRange(
              //   new MenuModel {
              //     StoreID = 1,
              //     PizzaID = 1
              //   }, new MenuModel {
              //     StoreID = 1,
              //     PizzaID = 2
              //   }, new MenuModel {
              //     StoreID = 1,
              //     PizzaID = 3
              //   }, new MenuModel {
              //     StoreID = 2,
              //     PizzaID = 2
              //   }, new MenuModel {
              //     StoreID = 2,
              //     PizzaID = 3
              //   }, new MenuModel {
              //     StoreID = 2,
              //     PizzaID = 4
              //   }, new MenuModel {
              //     StoreID = 2,
              //     PizzaID = 5
              //   }
              // );
              // context.Pizzas.AddRange(
              //   new PizzaModel {
              //     Name = "Supreme",
              //     Cost = 15.00M,
              //     DefaultToppings = "1,2,3,4,5",
              //     DefaultCrustID = 1
              //   }, new PizzaModel {
              //     Name = "Meatzza",
              //     Cost = 10.00M,
              //     DefaultToppings = "6,7,3,2",
              //     DefaultCrustID = 2
              //   }, new PizzaModel {
              //     Name = "Pepperoni",
              //     Cost = 5.00M,
              //     DefaultToppings = "3,8",
              //     DefaultCrustID = 3
              //   }, new PizzaModel {
              //     Name = "Cheese",
              //     Cost = 2.00M,
              //     DefaultToppings = "8,9",
              //     DefaultCrustID = 4
              //   }, new PizzaModel {
              //     Name = "Bread",
              //     Cost = 1.00M,
              //     DefaultToppings = "3,8",
              //     DefaultCrustID = 5
              //   }
              // );
              // context.Stores.AddRange(
              //   new StoreModel {
              //     Name = "Fricano's Pizza"
              //   }, new StoreModel {
              //     Name = "Hungry Howie's"
              //   }
              // );
              // context.Toppings.AddRange(
              //   new ToppingModel {
              //     Name = "Black Olives"
              //   }, new ToppingModel {
              //     Name = "Sausage"
              //   }, new ToppingModel {
              //     Name = "Pepperoni"
              //   }, new ToppingModel {
              //     Name = "Mushroom"
              //   }, new ToppingModel {
              //     Name = "Onion"
              //   }, new ToppingModel {
              //     Name = "Bacon"
              //   }, new ToppingModel {
              //     Name = "Ham"
              //   }, new ToppingModel {
              //     Name = "Mozzarella Cheese"
              //   }, new ToppingModel {
              //     Name = "Provolone Cheese"
              //   }
              // );

              // foreach (CrustModel crust in context.Crust.ToList()) {
              //   context.Crust.Remove(crust);
              // }
              // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Crust', RESEED, 0)"); // resets the identity key to 0 so that a new record is set to 1

              // foreach (MenuModel menu in context.Menu.ToList()) {
              //   context.Menu.Remove(menu);
              // }
              // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Menu', RESEED, 0)");

              // foreach (OrderModel order in context.Orders.ToList()) {
              //   context.Orders.Remove(order);
              // }
              // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Orders', RESEED, 0)");

              // foreach (PizzaModel pizza in context.Pizzas.ToList()) {
              //   context.Pizzas.Remove(pizza);
              // }
              // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Pizzas', RESEED, 0)");

              // foreach (StoreModel store in context.Stores.ToList()) {
              //   context.Stores.Remove(store);
              // }
              // context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Stores', RESEED, 0)");
              
              // context.SaveChanges();
            }
          }
        }

        public static void Main(string[] args) {
          IHost host = CreateHostBuilder(args).Build();
          Temp(host);
          CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
