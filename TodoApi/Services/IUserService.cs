using TodoApi.Dtos.Users;

namespace TodoApi.Services
{
  public interface IUserService
  {
    Task<UserDto> Create(UserRegisterInput input);
    Task<bool> CheckIfUserExists(string email);
    Task<UserDto> GetUser(string email);
    Task<bool> VerifyUserPassword(UserInput input);
  }
}
