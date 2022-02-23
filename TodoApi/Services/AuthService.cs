using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

namespace TodoApi.Services
{
  public class AuthService : IAuthService
  {
    private readonly IConfiguration configuration;

    public AuthService(IConfiguration configuration)
    {
      this.configuration = configuration;
    }

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512())
      {
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
      }
    }

    public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
      using (var hmac = new HMACSHA512(passwordSalt))
      {
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(passwordHash);
      }
    }

    public string CreateToken(string name, HashSet<Role> roles)
    {
      List<Claim> claims = new List<Claim>
      {
        new Claim(ClaimTypes.Name, name),
      };

      foreach (var role in roles)
      {
        #pragma warning disable CS8604
        claims.Add(new Claim(ClaimTypes.Role, Enum.GetName(typeof(Role), role)));
        #pragma warning restore CS8604
      }

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:Secret").Value));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var token = new JwtSecurityToken(
        claims: claims,
        expires: DateTime.Now.AddDays(1),
        signingCredentials: creds
      );

      var jwt = new JwtSecurityTokenHandler().WriteToken(token);
      return jwt;
    }
  }
}
