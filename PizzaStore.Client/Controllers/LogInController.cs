using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;
using PizzaStore.Storing.Repositories;

namespace PizzaStore.Client.Controllers {
  public class LogInController : Controller {
    // private readonly PizzaStoreDbContext _db;
    private readonly PizzaRepository _repo;

    public LogInController(PizzaRepository pizzaRepo) { // dependency injection handled by dotnet will pass the active DbContext instance here
      // _db = context;
      _repo = pizzaRepo;
    }

    [HttpGet]
    public IActionResult Prompt() {
      return View(new LogInViewModel());
    }

    [HttpGet]
    public string Manual(string id) {
      return id;
    }

    [HttpPost]
    public IActionResult Auto(LogInViewModel model) {
      int parsedID;
      if (!int.TryParse(model.IDInput, out parsedID)) {
        model.ReasonForError = "Invalid ID was given. You must type in a positive integer for an ID. Decimals or text are not allowed.";
        return View("Prompt", model);
      } else if (parsedID < 0) {
        model.ReasonForError = "A negative integer was entered in for the ID. Please enter a positive integer for an ID.";
        return View("Prompt", model);
      } else if (parsedID == 0) {
        model.ReasonForError = "Zero was entered in for the ID which is not positive. Please enter a positive integer for an ID.";
        return View("Prompt", model);
      }

      if (model.UserOrStore <= 0) {
        model.ReasonForError = "Please select whether you are a user or a store";
        return View("Prompt", model);
      } else if (model.UserOrStore > 2) {
        Console.WriteLine($"Invalid option for store/user selection; expected 0 or 1, got {model.UserOrStore}");
        model.ReasonForError = "There was an error processing your request. Please try again.";
        return View("Prompt", model);
      }
      
      TempData["IsUser"] = model.UserOrStore == 1;
      TempData.Keep("IsUser");
      
      if (model.UserOrStore == 1) {
        UserModel matchingUser = _repo.GetUser(parsedID);
        if (matchingUser == null) {
          model.IsUser = true;
          return View("DoesNotExist", model);
        }

        UserViewModel userViewModel = new UserViewModel();
        userViewModel.Name = matchingUser.Name;
        userViewModel.Stores = _repo.GetStores();
        TempData["UserID"] = matchingUser.ID;
        
        return Redirect("/User/StoreSelection");
      } else {  // if model.StoreOrUser == 2; check moved here to remove 'not all code paths return a value' error
        StoreModel matchingStore = _repo.GetStore(parsedID);
        if (matchingStore == null) {
          model.ReasonForError = "A store with this ID does not exist";
          return View("Prompt", model);
        }
        
        TempData["StoreID"] = matchingStore.ID;
        TempData["StoreName"] = matchingStore.Name;
        return Redirect("/Store/Store");
      }
    }
  
    public IActionResult CreateNew(LogInViewModel model) {
      model.IsUser = (bool) TempData["IsUser"];

      if (model.IsUser) {
        int newUserID = _repo.AddUser(model.NewName);
        if (newUserID == -1) {
          TempData.Keep("IsUser");
          model.ReasonForError = "There was an error processing your request. Please try again.";
          return View("DoesNotExist", model);
        }

        UserViewModel userViewModel = new UserViewModel();
        userViewModel.Name = model.NewName;
        userViewModel.ID = newUserID;

        return View("NewAdded", userViewModel);
      } else {
        TempData.Keep("IsUser");
        Console.WriteLine("Someone is attempting to create a store");
        model.ReasonForError = "There was an error processing your request. Please try again later.";
        return View("DoesNotExist", model);
      }
    }
  }
}