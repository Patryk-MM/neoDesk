using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.DTOs.Auth;
using neoDesk.Server.Models;

namespace neoDesk.Server.Services;

public class AuthService : IAuthService
{
    private readonly NeoDeskDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(NeoDeskDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // ... fragment pliku neoDesk.Server/Services/AuthService.cs

    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDto)
    {
        Console.WriteLine($"Login attempt for: {loginDto.Email}");

        var user = await GetUserByEmailAsync(loginDto.Email);
        Console.WriteLine($"User found: {user != null}");

        if (user == null || !user.IsActive)
        {
            Console.WriteLine("User not found or inactive");
            return null;
        }

        var passwordVerify = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
        Console.WriteLine($"Password verify: {passwordVerify}");

        // --- BRAKUJĄCY FRAGMENT ---
        if (!passwordVerify)
        {
            Console.WriteLine("Invalid password");
            return null;
        }
        // ---------------------------

        // Update last login
        user.LastLoginAt = DateTime.Now;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);

        return new AuthResponseDTO
        {
            Token = token,
            User = MapToUserDTO(user),
            ExpiresAt = DateTime.Now.AddHours(24)
        };
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UserDTO MapToUserDTO(User user)
    {
        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}