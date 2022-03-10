using TodoApi.Models.Users.Dtos;
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

    public async Task<UserDto> Create(UserRegisterInput input)
    {
      if (input.Password != input.PasswordConfirm)
      {
        throw new ApplicationException("Password confirmation does not match password.");
      }

      var userExists = await GetUser(input.Email);
      if (userExists != null)
      {
        throw new ApplicationException($"User '{input.Email}' already exists");
      }

      authService.CreatePasswordHash(input.Password, out byte[] passwordHash, out byte[] passwordSalt);
      var user = new User(input.Email, passwordHash, passwordSalt);

      // Add to role when roles exist

      await repository.AddAsync(user);

      return MapEntityToOutputDto(user);
    }

    public async Task<UserDto> GetUser(string email)
    {
      var user = await repository.FindAsync(x => x.Email == email);
      if (user != null)
      {
        return MapEntityToOutputDto(user);
      }
      #pragma warning disable CS8603
      return null;
      #pragma warning restore CS8603
    }

    public async Task<bool> VerifyUserPassword(UserInput input)
    {
      var user = await repository.FindAsync(x => x.Email == input.Email);
      return authService.VerifyPasswordHash(input.Password, user.PasswordHash, user.PasswordSalt);
    }

    // TODO make a generic entity mapper
    private UserDto MapEntityToOutputDto(User user)
    {
      return new UserDto()
      {
        Email = user.Email,
        Roles = user.Roles
      };
    }
  }
}
