namespace PizzaStore.Domain.Models {
  public class PizzaModel : AModel {
    public string Name { get; set; }
    public string Crust { get; set; }
    public string Toppings { get; set; }
    public decimal Cost { get; set; }
    public override string ToString() {
      string output = $"{Name} pizza with {Crust} crust and ";
      if (Toppings != null) {
        output += Toppings + " toppings";
      } else {
        output += "no toppings";
      }
      return output;
    }
  }
}