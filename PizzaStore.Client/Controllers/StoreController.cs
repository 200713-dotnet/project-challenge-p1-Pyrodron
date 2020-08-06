using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;

namespace PizzaStore.Client.Controllers {
  public class StoreController : Controller {
    private readonly PizzaStoreDbContext _db;
    private int userLoggedIn {
      get {
        TempData.Keep("UserID");
        return (int) TempData["UserID"];
      }
    }
    
    public StoreController(PizzaStoreDbContext context) { // dependency injection handled by dotnet will pass the active DbContext instance here
      _db = context;
    }

    [HttpGet]
    public IActionResult Visit(int ID) {
      Store store;
      try {
        store = _db.Stores.Where(s => s.ID == ID).SingleOrDefault();
      } catch (SqlException e) {
        if (e.Message.Contains("server was not found")) {
          Console.WriteLine("Could not connect to the SQL database");
          // ERROR
          return null;
        }
        throw e;
      }
      if (store == null) {
        // ERROR
      }

      List<Menu> items = _db.Menu.Where(m => m.StoreID == ID).ToList();
      List<Pizza> pizzas = new List<Pizza>();
      List<CheckModel> pizzasToSelectFrom = new List<CheckModel>();
      foreach (Menu item in items) {
        Pizza pizza = _db.Pizzas.Where(p => p.ID == item.PizzaID).SingleOrDefault();
        if (pizza == null) {
          Console.WriteLine($"Unknown pizza found with ID {item.PizzaID} from store {item.StoreID} at menu ID {item.ID}");
        } else {
          pizzas.Add(pizza);
          pizzasToSelectFrom.Add(new CheckModel{ID=pizza.ID, Name=pizza.Name, Checked=false, Cost=pizza.Cost});
        }
      }

      StoreViewModel model = new StoreViewModel();
      model.StoreName = store.Name;
      model.Menu = pizzasToSelectFrom;
      try {
      _ = userLoggedIn; // keeps the session data alive
      } catch (NullReferenceException) {
        // people can view menus if they're not logged in, but not order
      }

      TempData["StoreID"] = store.ID;
      TempData.Keep("StoreID");

      return View(model);
    }

    [HttpPost]
    public IActionResult SubmitOrder(StoreViewModel model) {
      int storeID = (int) TempData["StoreID"];
      Store store = _db.Stores.Where(s => s.ID == storeID).SingleOrDefault();
      model.StoreName = store.Name;

      try {
        _ = userLoggedIn;
      } catch (NullReferenceException) {
        model.ReasonForError = "You are not logged into the system. You will only be able to view menus until you return to the main page and log in.";
        return View("Visit", model);
      }

      string output = "";
      foreach (CheckModel selectedPizza in model.Menu) {
        output += $"{selectedPizza.ID} {selectedPizza.Name} {selectedPizza.Checked} {selectedPizza.SelectedSize} {selectedPizza.Cost} {selectedPizza.Quantity}<br>";
        if (selectedPizza.Checked) {
          string size = selectedPizza.SelectedSize.ToString().ToLower();
          if (!Regex.IsMatch(size, "small|medium|large")) {
            model.ReasonForError = $"Invalid size on pizza {selectedPizza.Name}";
            return View("Visit", model);
          }
          if (selectedPizza.Quantity == 0) {
            model.ReasonForError = $"{selectedPizza.Name} pizza must have a quantity greater than 0 if selected to be ordered";
            return View("Visit", model);
          } else if (selectedPizza.Quantity < 0) {
            model.ReasonForError = $"{selectedPizza.Name} pizza must have a positive quantity greater";
            return View("Visit", model);
          }
          
          Pizza pizza = _db.Pizzas.Where(p => p.ID == selectedPizza.ID).SingleOrDefault();
          _db.Orders.Add(new Order{
            StoreID = storeID,
            PizzaID = pizza.ID,
            UserID = userLoggedIn,
            Created = DateTime.Now,
            Quantity = selectedPizza.Quantity,
            TotalCost = pizza.Cost * (decimal) selectedPizza.Quantity,
            Size = size.ToUpper()[0]
          });
        }
      }
      _db.SaveChanges();

      return View("Submitted");
    }
  
    [HttpPost]
    public IActionResult BackToStoreSelection() {
      return Redirect("/User/StoreSelection");
    }
  }
}