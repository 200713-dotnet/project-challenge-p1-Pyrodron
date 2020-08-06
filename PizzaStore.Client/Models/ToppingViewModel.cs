using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class ToppingViewModel {
    public int ID { get; set; }
    public string Name { get; set; }
    public bool IsSelected { get; set; }
  }
}