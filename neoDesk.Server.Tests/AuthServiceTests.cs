using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs.Auth;
using neoDesk.Server.Models;
using neoDesk.Server.Services;

namespace neoDesk.Server.Tests.Services;

public class AuthServiceTests
{
    private readonly NeoDeskDbContext _context;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        var options = new DbContextOptionsBuilder<NeoDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NeoDeskDbContext(options);

        // Mockowanie konfiguracji (JWT Settings)
        _mockConfig = new Mock<IConfiguration>();
        var mockJwtSection = new Mock<IConfigurationSection>();
        mockJwtSection.Setup(s => s["SecretKey"]).Returns("SuperTajnyKluczDoTestowMusiBycDlugi123!");
        mockJwtSection.Setup(s => s["Issuer"]).Returns("neoDesk");
        mockJwtSection.Setup(s => s["Audience"]).Returns("neoDeskUsers");
        _mockConfig.Setup(c => c.GetSection("JwtSettings")).Returns(mockJwtSection.Object);

        _authService = new AuthService(_context, _mockConfig.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsTokenAndUser()
    {
        // Arrange
        string password = "password123";
        string email = "test@neodesk.com";
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            Name = "Test User",
            PasswordHash = passwordHash,
            Role = UserRole.EndUser,
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDTO { Email = email, Password = password };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().NotBeNull();
        result!.Token.Should().NotBeNullOrEmpty();
        result.User.Email.Should().Be(email);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        string email = "test@neodesk.com";
        var user = new User
        {
            Email = email,
            Name = "Test User",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("correctPassword"),
            IsActive = true
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDTO { Email = email, Password = "wrongPassword" };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WithInactiveUser_ReturnsNull()
    {
        // Arrange
        string password = "password123";
        string email = "inactive@neodesk.com";
        var user = new User
        {
            Email = email,
            Name = "Inactive User",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            IsActive = false // Użytkownik nieaktywny
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var loginDto = new LoginDTO { Email = email, Password = password };

        // Act
        var result = await _authService.LoginAsync(loginDto);

        // Assert
        result.Should().BeNull();
    }
}