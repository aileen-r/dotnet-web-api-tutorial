using TodoApi.Dtos.Users;
using TodoApi.Models.Users;

namespace TodoApi.Services
{
  public interface IUserService
  {
    Task<User> Create(UserInput input);
  }
}
