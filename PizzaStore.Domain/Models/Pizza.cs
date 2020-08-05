namespace PizzaStore.Domain.Models {
  public class Pizza : Model {
    public string Name { get; set; }
    public string Crust { get; set; }
    public string Toppings { get; set; }
    public decimal Cost { get; set; }
  }
}