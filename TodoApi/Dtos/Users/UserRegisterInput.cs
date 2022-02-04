namespace TodoApi.Dtos.Users
{
  public class UserRegisterInput : UserInput
  {
    public string PasswordConfirm { get; set; } = string.Empty;
  }
}
