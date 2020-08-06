using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;

namespace PizzaStore.Client.Controllers {
  public class OrderController : Controller {
    private readonly PizzaStoreDbContext _db;
    private int userLoggedIn {
      get {
        TempData.Keep("UserID");
        return (int) TempData["UserID"];
      }
    }
    public OrderController(PizzaStoreDbContext context) { // dependency injection handled by dotnet will pass the active DbContext instance here
      _db = context;
    }

    [HttpGet]
    public IActionResult OrderHistory(OrderViewModel model) {
      List<OrderModel> orders;
      try {
        _ = userLoggedIn; // exception not caught if you just use uLI
        orders = _db.Orders.Where(o => o.UserID == userLoggedIn).ToList();
      } catch (NullReferenceException) {
        model.ReasonForError = "You are not logged in. Please return to the main page to login and try again.";
        return View("Error", model);
      }
      Dictionary<int, Tuple<DateTime, StringBuilder, string, int, decimal, string>> orderDisplay = new Dictionary<int, Tuple<DateTime, StringBuilder, string, int, decimal, string>>(); // created, pizza list, crust string, quantity, cost, store name
      foreach (OrderModel order in orders) {
        PizzaModel pizza = _db.Pizzas.Where(p => p.ID == order.PizzaID).SingleOrDefault();
        string size = order.Size;
        try {
          orderDisplay[order.ID].Item2.Append($", {size} {pizza.ToString()}");
        } catch (KeyNotFoundException) {
          StringBuilder sb = new StringBuilder();
          sb.Append($"{size} {pizza.ToString()}");
          CrustModel crust = _db.Crust.Where(c => c.ID == order.CrustID).SingleOrDefault();
          StoreModel store = _db.Stores.Where(s => s.ID == order.StoreID).SingleOrDefault();
          orderDisplay.Add(order.ID, new Tuple<DateTime, StringBuilder, string, int, decimal, string>(order.Created, sb, crust.Name, order.Quantity, order.TotalCost, store.Name));
        }
      }

      model.OrderHistory = orderDisplay;
      return View(model);
    }

    [HttpPost]
    public IActionResult BackToSelection() {
      UserViewModel userViewModel = new UserViewModel();
      userViewModel.Name = _db.Users.Where(u => u.ID == userLoggedIn).SingleOrDefault().Name;
      userViewModel.Stores = _db.Stores.ToList();

      return View("../User/StoreSelection", userViewModel);
    }
  }
}