using neoDesk.Server.DTOs.Auth;
using neoDesk.Server.Models;

namespace neoDesk.Server.Services;

public interface IAuthService
{
    Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDto);
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    string GenerateJwtToken(User user);
}