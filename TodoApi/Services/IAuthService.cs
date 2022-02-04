namespace TodoApi.Services
{
  public interface IAuthService
  {
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
  }
}
