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
      StoreModel store;
      try {
        store = _db.Stores.Where(s => s.ID == ID).SingleOrDefault();
      } catch (SqlException e) {
        if (e.Message.Contains("server was not found")) {
          Console.WriteLine("Could not connect to the SQL database");
          StoreViewModel thisModel = new StoreViewModel();
          thisModel.ReasonForError = "An internal error has occured. Please return to the main page and try again.";
          return View("Error", thisModel);
        }
        throw e;
      }
      if (store == null) {
        StoreViewModel thisModel = new StoreViewModel();
        thisModel.ReasonForError = $"A store with an ID of {ID} does not exist. Please enter a different ID from the URL, or select a store from the selection page after logging in.";
        return View("Error", thisModel);
      }

      List<MenuModel> items = _db.Menu.Where(m => m.StoreID == ID).ToList();
      List<PizzaModel> pizzas = new List<PizzaModel>();
      List<CheckModel> pizzasToSelectFrom = new List<CheckModel>();
      foreach (MenuModel item in items) {
        PizzaModel pizza = _db.Pizzas.Where(p => p.ID == item.PizzaID).SingleOrDefault();
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
    public IActionResult SubmitOrder(StoreViewModel viewModel) {
      int storeID;
      try {
        _ = userLoggedIn;
        storeID = (int) TempData["StoreID"];
      } catch (NullReferenceException) {
        viewModel.ReasonForError = "You are not logged into the system. You will only be able to view menus until you return to the main page and log in.";
        return View("Visit", viewModel);
      }

      StoreModel store = _db.Stores.Where(s => s.ID == storeID).SingleOrDefault();
      viewModel.StoreName = store.Name;

      // string output = "";
      decimal overallCost = 0.00M;
      foreach (CheckModel selectedPizza in viewModel.Menu) {
        // output += $"{selectedPizza.ID} {selectedPizza.Name} {selectedPizza.Checked} {selectedPizza.SelectedSize} {selectedPizza.Cost} {selectedPizza.Quantity}<br>";
        if (selectedPizza.Checked) {
          string size = selectedPizza.SelectedSize.ToString().ToLower();
          if (!Regex.IsMatch(size, "small|medium|large")) {
            viewModel.ReasonForError = $"Invalid size on pizza {selectedPizza.Name}";
            return View("Visit", viewModel);
          }
          if (selectedPizza.Quantity == 0) {
            viewModel.ReasonForError = $"{selectedPizza.Name} pizza must have a quantity greater than 0 if selected to be ordered";
            return View("Visit", viewModel);
          } else if (selectedPizza.Quantity < 0) {
            viewModel.ReasonForError = $"{selectedPizza.Name} pizza must have a positive quantity greater";
            return View("Visit", viewModel);
          }
          
          PizzaModel pizza = _db.Pizzas.Where(p => p.ID == selectedPizza.ID).SingleOrDefault();
          decimal costOfThesePizzas = pizza.Cost * (decimal) selectedPizza.Quantity;
          _db.Orders.Add(new OrderModel{
            StoreID = storeID,
            PizzaID = pizza.ID,
            UserID = userLoggedIn,
            Created = DateTime.Now,
            Quantity = selectedPizza.Quantity,
            TotalCost = costOfThesePizzas,
            Size = size.ToUpper()[0]
          });
          overallCost += costOfThesePizzas;
        }
      }
      if (overallCost > 250.00M) {
        viewModel.ReasonForError = "This order exceeds $250. Please remove some pizzas, then try again.";
        return View("Visit", viewModel);
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