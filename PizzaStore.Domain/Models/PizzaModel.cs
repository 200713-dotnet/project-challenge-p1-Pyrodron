namespace PizzaStore.Domain.Models {
  public class PizzaModel : AModel {
    public string Name { get; set; }
    public int DefaultCrustID { get; set; }
    public string Toppings { get; set; }
    public decimal Cost { get; set; }
    public  override string ToString() {
      string output = $"{Name} pizza with {(Toppings != null ? Toppings : "no")} toppings";
      return output;
    }
  }
}