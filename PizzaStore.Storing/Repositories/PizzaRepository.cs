using System;
using System.Collections.Generic;
using System.Linq;
using PizzaStore.Domain.Models;

namespace PizzaStore.Storing.Repositories {
  public class PizzaRepository {

    private PizzaStoreDbContext _db;
    public PizzaRepository(PizzaStoreDbContext context) {
      _db = context;
    }

    public List<StoreModel> GetStores() {
      return _db.Stores.ToList();
    }

    public StoreModel GetStore(int ID) {
      return _db.Stores.Where(s => s.ID == ID).SingleOrDefault();
    }
    
    public UserModel GetUser(int ID) {
      return _db.Users.Where(u => u.ID == ID).SingleOrDefault();
    }

    public int AddUser(string name) {
      try {
        _db.Users.Add(new UserModel {
          Name = name
        });
        _db.SaveChanges();
        return _db.Users.Max(u => u.ID);
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        return -1;
      }
    }

    public List<OrderModel> GetOrdersForUser(int ID) {
      return _db.Orders.Where(o => o.UserID == ID).ToList();
    }

    public ToppingModel GetTopping(int ID) {
      return _db.Toppings.Where(t => t.ID == ID).SingleOrDefault();
    }

    public CrustModel GetCrust(int ID) {
      return _db.Crust.Where(c => c.ID == ID).SingleOrDefault();
    }

    public PizzaModel GetPizza(int ID) {
      return _db.Pizzas.Where(p => p.ID == ID).SingleOrDefault();
    }

    public List<MenuModel> GetMenu(int ID) {
      return _db.Menu.Where(m => m.StoreID == ID).ToList();
    }

    public List<ToppingModel> GetToppings() {
      return _db.Toppings.ToList();
    }

    public List<CrustModel> GetCrusts() {
      return _db.Crust.ToList();
    }

    public int GetNextOrderNumber() {
      try {
        return _db.Orders.Max(o => o.OrderID) + 1;
      } catch (InvalidOperationException e) { // orders table might be empty - dotnet still prints the exceptions even when caught
        if (!e.Message.Contains("Sequence contains no elements")) {
          throw e;
        }
        return 1;
      }
    }

    public bool AddOrder(OrderModel order) {
      try {
        _db.Orders.Add(order);
        _db.SaveChanges();
        return true;
      } catch (Exception e) {
        Console.WriteLine(e.Message);
        Console.WriteLine(e.StackTrace);
        return false;
      }
    }

    public List<OrderModel> GetOrdersForStore(int ID) {
      return _db.Orders.Where(o => o.StoreID == ID).ToList();
    }

    public List<OrderModel> GetOrdersForStoreAndUser(int storeID, int userID) {
      return _db.Orders.Where(o => o.StoreID == storeID).Where(o => o.UserID == userID).ToList();
    }

    public List<OrderModel> GetOrders() {
      return _db.Orders.ToList();
    }
  }
}

// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Microsoft.EntityFrameworkCore;
// using domain = PizzaBox.Domain.Models;

// namespace PizzaBox.Storing.Repositories {
//   // CRUD methods for database

//   public class PizzaRepository {
//     private PizzaProjectContext db = new PizzaProjectContext();
//     Dictionary<int, List<domain.Pizza>> menu = new Dictionary<int, List<domain.Pizza>>(); // storeId, pizza list
//     Dictionary<int, domain.Pizza> pizzas = new Dictionary<int, domain.Pizza>(); // pizzaId, pizza

//     // store id, <user id, <order id, order>>>
//     Dictionary<int, Dictionary<int, Dictionary<int, domain.Order>>> orders = 
//       new Dictionary<int, Dictionary<int, Dictionary<int, domain.Order>>>();
//     Dictionary<int, domain.Store> stores = new Dictionary<int, domain.Store>(); // storeID, store

//     public PizzaRepository() {
      
//       foreach (Store store in db.Store.ToList()) {
//         stores.Add(store.Id, new domain.Store() {
//           name = store.Name,
//           id = store.Id,
//           // add menus later
//           // add orders later
//         });
//       }
//       pizzas = ReadAllPizzas();

//       // populate menus
//       foreach (Menu menuItem in db.Menu.ToList()) {
//         int storeID = (int) menuItem.StoreId;
//         int pizzaID = (int) menuItem.PizzaId;
//         try {
//           menu[storeID].Add(pizzas[pizzaID]);
//         } catch (KeyNotFoundException) {
//           menu.Add(storeID, new List<domain.Pizza>{ pizzas[pizzaID] });
//         }
//       }
      
//       foreach (PizzaOrder order in db.PizzaOrder.ToList()) {
//         AddOrder(order);
//       }

//       foreach (int storeID in stores.Keys) {
//         try {
//           stores[storeID].completedOrders = orders[storeID];
//         } catch (KeyNotFoundException) {
//           // a store may not have any orders placed
//         }
//         try {
//           stores[storeID].menu = menu[storeID].ToArray();
//         } catch (KeyNotFoundException) {
//           // a store may not have pizzas to sell. maybe they're just selling breadsticks?
//         }
//       }
//     }

//     private void AddOrder(PizzaOrder order) {
//       int storeID = (int) order.StoreId;
//       int userID = (int) order.UserId;
//       int orderID = (int) order.OrderId;
//       int pizzaID = (int) order.PizzaId;

//       try {
//         _ = orders[storeID];
//       } catch (KeyNotFoundException) {
//         orders.Add(storeID, new Dictionary<int, Dictionary<int, domain.Order>>());
//         // Console.WriteLine("S" + storeID); // working
//       }
//       try {
//         _ = orders[storeID][userID];
//       } catch (KeyNotFoundException) {
//         orders[storeID].Add(userID, new Dictionary<int, domain.Order>());
//         // Console.WriteLine($"S{storeID} U{userID}");  // working
//       }
//       domain.Order domainOrder;
//       try {
//         domainOrder = orders[storeID][userID][orderID];
//       } catch (KeyNotFoundException) {
//         domainOrder = new domain.Order{
//           created = order.WhenOrdered,
//           store = stores[storeID],
//           user = order.UserId
//         };
//         orders[storeID][userID].Add(orderID, domainOrder);
//         // Console.WriteLine($"S{storeID} U{userID} O{orderID}"); // working
//       }
//       domain.Pizza pizza = pizzas[pizzaID];
//       pizza.size = order.Size[0];
//       domainOrder.AddPizza(pizzas[pizzaID], true);
//       orders[storeID][userID][orderID] = domainOrder;
//     }

//     public Dictionary<int, domain.Pizza> ReadAllPizzas() {
//       Dictionary<int, domain.Pizza> pizzas = new Dictionary<int, domain.Pizza>();
//       foreach (Pizza pizza in db.Pizza.ToList()) {
//         pizzas.Add(pizza.Id, new domain.Pizza(){
//           id = pizza.Id,
//           name = pizza.Name,
//           cost = (double) pizza.Price,
//           toppings = pizza.Toppings == null? new string[0] : pizza.Toppings.Split(','),
//           crust = pizza.Crust
//         });
//       }
//       return pizzas;
//     }
  
//     public Dictionary<int, domain.Store> GetStores() {
//       return new Dictionary<int, domain.Store>(stores);
//     }

//     public Dictionary<int, domain.Pizza> GetPizzas() {
//       return new Dictionary<int, domain.Pizza>(pizzas);
//     }

//     public Dictionary<int, domain.Order> GetOrders(int userID) {
//       Dictionary<int, domain.Order> ordersForUser = new Dictionary<int, domain.Order>();
//       foreach (int storeID in orders.Keys) {
//         Dictionary<int, Dictionary<int, domain.Order>> ordersForStore = orders[storeID];
//         foreach (int orderID in ordersForStore[userID].Keys) {
//           ordersForUser.Add(orderID, ordersForStore[userID][orderID]);
//         }
//       }
//       return ordersForUser;
//     }

//     public void AddOrderToDB(domain.Order order) {
//       int newOrderNumber = db.PizzaOrder.Max(p => p.OrderId) + 1;
//       List<domain.Order> ordersToAdd = new List<domain.Order>();
//       foreach (domain.Pizza pizza in order.pizzas) {
//         int store_ID = order.store.id;
//         int user_ID = order.user;
//         PizzaOrder dbOrder = new PizzaOrder {
//           OrderId = newOrderNumber,
//           StoreId = store_ID,
//           PizzaId = pizza.id,
//           UserId = user_ID,
//           WhenOrdered = (DateTime) order.created,
//           TotalCost = (float) order.totalCost,
//           Size = pizza.size.ToString()
//         };
//         AddOrder(dbOrder);
//         db.PizzaOrder.Add(dbOrder);
//       }
//       db.SaveChanges();
//     }
//   }
// }