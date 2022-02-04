using System;
using TodoApi.Dtos.Users;
using TodoApi.Models.Users;

namespace TodoApi.Services
{
  public class UserService : IUserService
  {
    private readonly IAuthService authService;
    public UserService(IAuthService authService)
    {
      this.authService = authService;
    }

    public async Task<User> Create(UserInput input)
    {
      if (input.Password != input.PasswordConfirm)
      {
        // TODO return error
      }

      // TODO: check user exists

      authService.CreatePasswordHash(input.Password, out byte[] passwordHash, out byte[] passwordSalt);
      var user = new User(input.Email, passwordHash, passwordSalt);

      // Add to role when roles exist
      // Save to respository

      // TODO map to output dto

      return user;
    }
  }
}
