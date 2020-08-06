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
              // context.Stores.AddRange(
              //   new Store {
              //     Name = "Fricano's Pizza"
              //   }, new Store {
              //     Name = "Hungry Howie's"
              //   }
              // );

              // context.Pizzas.AddRange(
              //   new Pizza {
              //     Name = "Supreme",
              //     Cost = 15.00M,
              //     Toppings = "black olives,sausage,pepperoni,mushroom,onion",
              //     Crust = "garlic butter"
              //   }, new Pizza {
              //     Name = "Meatzza",
              //     Cost = 10.00M,
              //     Toppings = "bacon,ham,pepperoni,sausage",
              //     Crust = "Stuffed"
              //   }, new Pizza {
              //     Name = "Pepperoni",
              //     Cost = 5.00M,
              //     Toppings = "pepperoni",
              //     Crust = "thick"
              //   }, new Pizza {
              //     Name = "Cheese",
              //     Cost = 2.00M,
              //     Toppings = null,
              //     Crust = "thin"
              //   }
              // );
              // context.Menu.AddRange(
              //   new Menu {
              //     StoreID = 1,
              //     PizzaID = 1
              //   }, new Menu {
              //     StoreID = 1,
              //     PizzaID = 2
              //   }, new Menu {
              //     StoreID = 1,
              //     PizzaID = 3
              //   }, new Menu {
              //     StoreID = 2,
              //     PizzaID = 2
              //   }, new Menu {
              //     StoreID = 2,
              //     PizzaID = 3
              //   }, new Menu {
              //     StoreID = 2,
              //     PizzaID = 4
              //   }
              // );
              // foreach (Order order in context.Orders.ToList()) {
              //   context.Orders.Remove(order);
              // }
              // context.SaveChanges();
            }
          }
        }

        public static void Main(string[] args) {
          IHost host = CreateHostBuilder(args).Build();
          // Temp(host);
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
