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

      List<OrderViewClass> orderHistory = new List<OrderViewClass>();
      foreach (OrderModel order in orders) {
        PizzaModel pizza = _db.Pizzas.Where(p => p.ID == order.PizzaID).SingleOrDefault();
        orderHistory.Add(new OrderViewClass{
          OrderID = order.OrderID,
          Created = order.Created,
          Pizzas = pizza.ToString(),
          Size = order.Size,
          Crust = _db.Crust.Where(c => c.ID == order.CrustID).SingleOrDefault().Name,
          Toppings = order.Toppings,
          Quantity = order.Quantity,
          Cost = order.TotalCost,
          StoreName = _db.Stores.Where(s => s.ID == order.StoreID).SingleOrDefault().Name
        });
      }
      model.OrderHistory = orderHistory;
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