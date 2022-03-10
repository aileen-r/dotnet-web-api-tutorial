using Microsoft.AspNetCore.Mvc;
using TodoApi.Models.Users.Dtos;
using TodoApi.Models.Users;
using TodoApi.Services;

namespace TodoApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IUserService userService;
    private readonly IAuthService authService;

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

      // TODO: for prod these should return the same error
      if (user == null)
      {
        return BadRequest("User not found.");
      }

      var correctPassword = await userService.VerifyUserPassword(input);
      if (!correctPassword)
      {
        return BadRequest("Incorrect password.");
      }

      string token = authService.CreateToken(user.Email, user.Roles);
      return Ok(token);
    }
  }
}
