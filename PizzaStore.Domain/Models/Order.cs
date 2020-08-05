using System;

namespace PizzaStore.Domain.Models {
  public class Order : Model {
    public int StoreID { get; set; }
    public int PizzaID { get; set; }
    public int UserID { get; set; }
    public DateTime Created { get; set; }
    public decimal TotalCost { get; set; }
    public char Size { get; set; }
  }
  
}