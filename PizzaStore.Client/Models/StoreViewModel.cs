using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class StoreViewModel {
    public int ID { get; set; }
    public string StoreName { get; set; }
    public int OptionSelected { get; set; }
    public List<CheckModel> Menu { get; set; }
    public string ReasonForError { get; set; }
    public List<SelectListItem> Crusts { get; set; }
    public List<ToppingModel> Toppings { get; set; }
    public string FilterHistoryToUser { get; set; }
    public List<OrderViewClass> OrderHistory { get; set; }
  }

  public class CheckModel {
    public int ID { get; set; }  
    public string Name { get; set; }  
    public bool Checked { get; set; }
    public Size SelectedSize { get; set; }
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
    public string SelectedCrust { get; set; } // crust ID to string
    public int DefaultCrust { get; set; }
    public ToppingViewModel[] SelectedToppings { get; set; }
  }

  public enum Size {
    Small, Medium, Large
  }
}