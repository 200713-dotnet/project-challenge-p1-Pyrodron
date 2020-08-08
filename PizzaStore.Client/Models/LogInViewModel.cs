namespace PizzaStore.Client.Models {
  public class LogInViewModel {
    public int UserOrStore { get; set; }
    public bool IsUser { get; set; }
    public string IDInput { get; set; }
    public string ReasonForInvalid { get; set; }
    public string NewName { get; set; }
    public string ReasonForError { get; set; }
  }
}