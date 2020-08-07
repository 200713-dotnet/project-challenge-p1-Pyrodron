using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;

namespace PizzaStore.Client.Controllers {
  public class UserController : Controller {
    private readonly PizzaStoreDbContext _db;
    private int userLoggedIn {
      get {
        TempData.Keep("UserID");
        return (int) TempData["UserID"];
      }
    }

    public UserController(PizzaStoreDbContext context) { // dependency injection handled by dotnet will pass the active DbContext instance here
      _db = context;
    }
    
    [HttpPost]
    public IActionResult AddUser(LogInViewModel model) {
      try {
        _db.Users.Add(new UserModel{
          Name = model.NewName
        });
        _db.SaveChanges();

        UserViewModel userViewModel = new UserViewModel();
        userViewModel.Name = model.NewName;
        userViewModel.ID = _db.Users.Max(u => u.ID);

        return View("NewUserAdded", userViewModel);
      } catch (Exception e) {
        Console.WriteLine($"{e.Message}\n{e.StackTrace}");
        UserViewModel thisModel = new UserViewModel();
        thisModel.ReasonForError = "An unknown error occured while adding a new user to the system. Please visit the main page and try again.";
        return View("Error", thisModel);
      }
    }

    public IActionResult StoreSelection() {
      UserViewModel model = new UserViewModel();
      model.Name = _db.Users.Where(u => u.ID == userLoggedIn).SingleOrDefault().Name;
      model.Stores = _db.Stores.ToList();

      return View(model);
    }

    public IActionResult OptionSelected(UserViewModel model) {
      try {
        _ = userLoggedIn;
      } catch (NullReferenceException) {
        model.ReasonForError = "You are not logged in. Please return to the main page, log in, and try again.";
        return View("Error", model);
      }
      int option = model.OptionSelected;
      if (option == 0) {
        Console.WriteLine("Impossible for store ID to be 0; User is probably not logged in");
        model.ReasonForError = "An invalid store selection has been made. This could be caused by being improperly logged in. Please return to the main page and try again.";
        return View("Error", model);
      } else if (option == -1) {
        return Redirect("/Order/OrderHistory");
      }
      StoreModel foundStore = null;
      foreach (StoreModel store in _db.Stores.ToList()) {
        if (store.ID == option) {
          foundStore = store;
          break;
        }
      }
      if (foundStore == null) {
        Console.WriteLine("Unknown store ID given; Could not find store");
        model.ReasonForError = "An invalid store selection has been made. This could be caused by being improperly logged in. Please return to the main page and try again.";
        return View("Error", model);
      }

      return Redirect($"/Store/Visit?ID={foundStore.ID}");
    }

    public IActionResult LogOut() {
      TempData.Remove("UserID");
      return Redirect("/");
    }
  }
}