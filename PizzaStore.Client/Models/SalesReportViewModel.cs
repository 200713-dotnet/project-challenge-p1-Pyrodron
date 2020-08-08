using System;
using System.Collections.Generic;
using PizzaStore.Domain.Models;

namespace PizzaStore.Client.Models {
  public class SalesReportViewModel {
    public string StoreName { get; set; }
    public int Interval { get; set; }
    public string ReasonForError { get; set; }
    public Dictionary<DateTime, OrderViewModel> SalesReport { get; set; }
  }
}