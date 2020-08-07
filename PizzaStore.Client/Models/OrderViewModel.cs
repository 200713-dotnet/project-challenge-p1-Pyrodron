using System;
using System.Collections.Generic;
using System.Text;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class OrderViewModel {
    public List<OrderViewClass> OrderHistory { get; set; }
    public string ReasonForError { get; set; }
  }

  public class OrderViewClass {
    public int ID { get; set; }
    public DateTime Created { get; set; }
    public string Pizzas { get; set; }
    public string Size { get; set; }
    public string Crust { get; set; }
    public string Toppings { get; set; }
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
    public string StoreName { get; set; }
  }
}
