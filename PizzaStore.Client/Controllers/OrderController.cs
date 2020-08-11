using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;
using PizzaStore.Storing.Repositories;

namespace PizzaStore.Client.Controllers {
  public class OrderController : Controller {
    private readonly PizzaRepository _repo;
    private int userLoggedIn {
      get {
        TempData.Keep("UserID");
        return (int) TempData["UserID"];
      }
    }
    public OrderController(PizzaRepository pizzaRepo) { // dependency injection handled by dotnet will pass the active DbContext instance here
      _repo = pizzaRepo;
    }

    [HttpGet]
    public IActionResult OrderHistory(OrderViewModel model) {
      List<OrderModel> orders;
      try {
        _ = userLoggedIn; // exception not caught if you just use uLI
        orders = _repo.GetOrdersForUser(userLoggedIn);
      } catch (NullReferenceException) {
        model.ReasonForError = "You are not logged in. Please return to the main page to login and try again.";
        return View("Error", model);
      }

      List<OrderViewClass> orderHistory = new List<OrderViewClass>();
      foreach (OrderModel order in orders) {
        StringBuilder toppings = new StringBuilder();
        foreach (string topping in order.Toppings.Split(',')) {
          int toppingID;
          if (!int.TryParse(topping, out toppingID)) {
            Console.WriteLine($"Database error: Expected integer for pizza ID, received {topping}");
            toppings.Append("Error, ");
            continue;
          }
          ToppingModel top = _repo.GetTopping(toppingID);
          toppings.Append($"{top.Name}, ");
        }
        toppings.Remove(toppings.Length - 2, 2);
        OrderViewClass orderView = new OrderViewClass{
          OrderID = order.OrderID,
          Created = order.Created,
          Size = order.Size,
          Crust = _repo.GetCrust(order.CrustID).Name,
          Toppings = toppings.ToString(),
          Quantity = order.Quantity,
          Cost = order.TotalCost,
          StoreName = _repo.GetStore(order.StoreID).Name
        };
        if (order.PizzaID == 0) {
          orderView.Pizza = "Custom";
        } else {
          try {
            orderView.Pizza = _repo.GetPizza(order.PizzaID).Name;
          } catch (NullReferenceException) {
            Console.WriteLine($"Database error: Could not find a pizza with ID {order.PizzaID} in the Pizza table");
            orderView.Pizza = "Error";
          }
        }
        orderHistory.Add(orderView);
      }
      model.OrderHistory = orderHistory;
      return View(model);
    }

    [HttpPost]
    public IActionResult BackToSelection() {
      UserViewModel userViewModel = new UserViewModel();
      userViewModel.Name = _repo.GetUser(userLoggedIn).Name;
      userViewModel.Stores = _repo.GetStores();

      return View("../User/StoreSelection", userViewModel);
    }
  }
}