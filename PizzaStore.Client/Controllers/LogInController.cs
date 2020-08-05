using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzaStore.Client.Models;
using PizzaStore.Domain.Models;
using PizzaStore.Storing;

namespace PizzaStore.Client.Controllers {
  public class LogInController : Controller {
    private readonly PizzaStoreDbContext _db;

    public LogInController(PizzaStoreDbContext context) { // dependency injection handled by dotnet will pass the active DbContext instance here
      _db = context;
    }

    [HttpGet]
    public IActionResult Prompt() {
      return View();
    }

    [HttpGet]
    public string Manual(string id) {
      return id;
    }

    [HttpPost]
    public IActionResult Auto(LogInViewModel model) {
      int parsedInput;
      if (int.TryParse(model.IDInput, out parsedInput)) {
        if (parsedInput < 0) {
          model.ReasonForInvalid = "A negative integer was entered in for the user ID.";
          return View("InvalidID", model);
        } else if (parsedInput == 0) {
          model.ReasonForInvalid = "Zero is not a positive integer.";
          return View("InvalidID", model);
        }
        User matchingUser = null;
        foreach (User user in _db.Users) {
          if (user.ID == parsedInput) {
            matchingUser = user;
            break;
          }
        }
        if (matchingUser == null) {
          return View("DoesNotExist", model);
        }

        UserViewModel userViewModel = new UserViewModel();
        userViewModel.Name = matchingUser.Name;
        userViewModel.Stores = _db.Stores.ToList();
        TempData["UserID"] = matchingUser.ID;
        
        return Redirect("/User/StoreSelection");
      } else {
        model.ReasonForInvalid = "Invalid input was given. Either text or a decimal number was entered in.";
        return View("InvalidID", model);
      }
    }
  }
}