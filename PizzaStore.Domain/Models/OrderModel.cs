using System;

namespace PizzaStore.Domain.Models {
  public class OrderModel : AModel {
    public int OrderID { get; set; }
    public int StoreID { get; set; }
    public int PizzaID { get; set; }
    public int UserID { get; set; }
    public DateTime Created { get; set; }
    public decimal TotalCost { get; set; }
    public string Size { get; set; }
    public int Quantity { get; set; }
    public int CrustID { get; set; }
    public string Toppings { get; set; }
  }
  
}