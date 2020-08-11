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