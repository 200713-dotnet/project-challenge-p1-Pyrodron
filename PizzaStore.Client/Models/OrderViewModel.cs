using System;
using System.Collections.Generic;
using System.Text;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class OrderViewModel {
    public Dictionary<int, Tuple<DateTime, StringBuilder, int, decimal, string>> OrderHistory { get; set; }
    public string ReasonForError { get; set; }
  }
}
