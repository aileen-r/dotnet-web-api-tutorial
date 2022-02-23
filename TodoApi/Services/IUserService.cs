using TodoApi.Dtos.Users;

namespace TodoApi.Services
{
  public interface IUserService
  {
    Task<UserDto> Create(UserRegisterInput input);
    Task<UserDto> GetUser(string email);
    Task<bool> VerifyUserPassword(UserInput input);
  }
}
