namespace TodoApi.Models.Exceptions
{
  public class TodoApiException : Exception
  {
    public string Title { get; set; } = String.Empty;

    public TodoApiException(string title)
    {
      this.Title = title;
    }

    public TodoApiException(string title, string? message) : base(message)
    {
      this.Title = title;
    }
  }
}
