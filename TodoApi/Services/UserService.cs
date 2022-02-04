using TodoApi.Dtos.Users;
using TodoApi.Models.Users;
using TodoApi.Repositories;

namespace TodoApi.Services
{
  public class UserService : IUserService
  {
    private IRepository<User> repository;
    private readonly IAuthService authService;

    public UserService(IRepository<User> repository, IAuthService authService)
    {
      this.repository = repository;
      this.authService = authService;
    }

    public async Task<User> Create(UserInput input)
    {
      if (input.Password != input.PasswordConfirm)
      {
        throw new Exception("Password confirmation does not match password.");
      }

      var userExists = await CheckIfUserExists(input.Email);
      if (userExists)
      {
        throw new Exception($"User '{input.Email}' already exists");
      }

      authService.CreatePasswordHash(input.Password, out byte[] passwordHash, out byte[] passwordSalt);
      var user = new User(input.Email, passwordHash, passwordSalt);

      // Add to role when roles exist

      await repository.AddAsync(user);

      // TODO map to output dto

      return user;
    }

    private async Task<bool> CheckIfUserExists(string email)
    {
      var existingUser = await repository.FindAsync(x => x.Email == email);
      if (existingUser == null)
      {
        return false;
      }
      return true;
    }
  }
}
