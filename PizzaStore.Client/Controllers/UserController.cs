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
    
    public IActionResult StoreSelection() {
      UserViewModel model = new UserViewModel();
      model.Name = _db.Users.Where(u => u.ID == userLoggedIn).SingleOrDefault().Name;
      model.Stores = _db.Stores.ToList();

      return View(model);
    }

    public IActionResult OptionSelected(UserViewModel model) {
      model.Name = _db.Users.Where(u => u.ID == userLoggedIn).SingleOrDefault().Name;
      model.Stores = _db.Stores.ToList();

      bool submitSelectionClicked = Request.Form["SubmitSelection"].ToString() != "";
      bool logOutClicked = Request.Form["LogOut"].ToString() != "";
      int buttonsClicked = (submitSelectionClicked ? 1 : 0) + (logOutClicked ? 1 : 0);
      
      if (buttonsClicked > 1) {
        Console.WriteLine("Multiple buttons registered as click on the store selection");
        model.ReasonForError = "There was a problem processing your request. Please try again.";
        return View(model);
      } else if (submitSelectionClicked) {
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
      } else if (logOutClicked) {
        TempData.Remove("UserID");
        return Redirect("/");
      } else {  // no buttons check is placed down here to remove the 'not all code paths return a value' error
        Console.WriteLine("Request sent but no buttons registered as clicked on the store selection");
        model.ReasonForError = "There was a problem processing your request. Please try again.";
        return View(model);
      }
    }
  }
}