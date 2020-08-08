using System;
using System.Collections.Generic;
using System.Text;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class OrderViewModel {
    public List<OrderViewClass> OrderHistory { get; set; }
    public string ReasonForError { get; set; }
    public int IntervalQuantity { get; set; }
    public decimal IntervalSales { get; set; }
  }

  public class OrderViewClass {
    public int UserID { get; set; }
    public int OrderID { get; set; }
    public DateTime Created { get; set; }
    public string Pizza { get; set; }
    public string Size { get; set; }
    public string Crust { get; set; }
    public string Toppings { get; set; }
    public int Quantity { get; set; }
    public decimal Cost { get; set; }
    public string StoreName { get; set; }
  }
}
