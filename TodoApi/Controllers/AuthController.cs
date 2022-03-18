using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.Users.Dtos;
using TodoApi.Models.Users;
using TodoApi.Services;
using TodoApi.Models.Exceptions;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUserService userService;
    private readonly IAuthService authService;
    private readonly bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    public AuthController(IUserService userService, IAuthService authService)
    {
      this.userService = userService;
      this.authService = authService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> Register(UserRegisterInput input)
    {
      var user = await userService.Create(input);

      return Ok(user);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(UserInput input)
    {
      var user = await userService.GetUser(input.Email);

      if (user == null)
      {
        if (isDevelopment)
        {
          throw new TodoApiException("Incorrect email address or password", $"The user '{input.Email}' does not exist.");
        }
        throw new TodoApiException("Incorrect email address or password");
      }

      var correctPassword = await userService.VerifyUserPassword(input);
      if (!correctPassword)
      {if (isDevelopment)
        {
          throw new TodoApiException("Incorrect email address or password", $"Password for user '{input.Email}' is incorrect.");
        }
        throw new TodoApiException("Incorrect email address or password");
      }

      string token = authService.CreateToken(user.Email, user.Roles);
      return Ok(token);
    }
  }
}
