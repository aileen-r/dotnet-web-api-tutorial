namespace TodoApi.Models.Users.Dtos
{
  public class UserRegisterInput : UserInput
  {
    public string PasswordConfirm { get; set; } = string.Empty;
  }
}
