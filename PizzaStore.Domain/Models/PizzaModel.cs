namespace PizzaStore.Domain.Models {
  public class PizzaModel : AModel {
    public string Name { get; set; }
    public int DefaultCrustID { get; set; }
    public string DefaultToppings { get; set; }
    public decimal Cost { get; set; }
    public  override string ToString() {
      return Name;
    }
  }
}