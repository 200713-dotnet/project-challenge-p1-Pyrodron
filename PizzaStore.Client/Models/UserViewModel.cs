using System.Collections.Generic;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class UserViewModel {
    public int ID { get; set; }
    public string Name { get; set; }
    List<Store> _stores = null;
    public List<Store> Stores {
      get {
        return _stores;
      }
      set {
        if (_stores == null) {
          _stores = value;
        }
      }
    }
    public int OptionSelected { get; set; }
    public string ReasonForError { get; set; }
  }
}