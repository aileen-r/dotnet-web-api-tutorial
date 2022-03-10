using TodoApi.Models.Users.Dtos;

namespace TodoApi.Services
{
  public interface IUserService
  {
    Task<UserDto> Create(UserRegisterInput input);
    Task<UserDto> GetUser(string email);
    Task<bool> VerifyUserPassword(UserInput input);
  }
}
