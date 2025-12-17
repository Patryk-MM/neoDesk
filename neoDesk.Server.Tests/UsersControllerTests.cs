using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neoDesk.Server.Controllers;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.Models;

namespace neoDesk.Server.Tests.Controllers;

public class UsersControllerTests
{
    private readonly NeoDeskDbContext _context;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        // Używamy unikalnej nazwy bazy dla każdego testu (Guid)
        var options = new DbContextOptionsBuilder<NeoDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NeoDeskDbContext(options);
        _controller = new UsersController(_context);
    }

    [Fact]
    public async Task CreateUser_ReturnsBadRequest_WhenEmailAlreadyExists()
    {
        // Arrange - dodajemy istniejącego użytkownika
        var existingUser = new User
        {
            Name = "Existing",
            Email = "duplicate@test.com",
            PasswordHash = "hash",
            Role = UserRole.EndUser
        };
        _context.Users.Add(existingUser);
        await _context.SaveChangesAsync();

        var newUserDto = new CreateUserDTO
        {
            Name = "New",
            Email = "duplicate@test.com", // Ten sam email
            Password = "pass",
            Role = "EndUser"
        };

        // Act
        var result = await _controller.CreateUser(newUserDto);

        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task GetAssignableUsers_ReturnsOnlyAdminsAndTechnicians()
    {
        // Arrange
        _context.Users.AddRange(
            new User { Name = "Admin", Role = UserRole.Admin, Email = "a@a.com", PasswordHash = "x", IsActive = true },
            new User { Name = "Tech", Role = UserRole.Technician, Email = "t@t.com", PasswordHash = "x", IsActive = true },
            new User { Name = "User", Role = UserRole.EndUser, Email = "u@u.com", PasswordHash = "x", IsActive = true },
            new User { Name = "InactiveTech", Role = UserRole.Technician, Email = "it@t.com", PasswordHash = "x", IsActive = false }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.GetAssignableUsers();

        // Assert
        var okResult = result.Result as OkObjectResult;
        var users = okResult!.Value as IEnumerable<UserDTO>;

        users.Should().HaveCount(2); // Tylko aktywny Admin i aktywny Technik
        users!.Should().Contain(u => u.Role == "Admin");
        users!.Should().Contain(u => u.Role == "Technician");
        users!.Should().NotContain(u => u.Role == "EndUser");
    }

    [Fact]
    public async Task DeleteUser_ReturnsBadRequest_IfUserHasTickets()
    {
        // Arrange
        var user = new User { Id = 10, Name = "User with Ticket", Email = "t@t.com", PasswordHash = "x" };
        _context.Users.Add(user);

        // Dodajemy ticket stworzony przez tego usera
        _context.Tickets.Add(new Ticket
        {
            Title = "Bug",
            Description = "Fix it",
            CreatedByUserId = 10
        });
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.DeleteUser(10);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
              .Which.Value.ToString().Should().Contain("Nie można usunąć użytkownika");
    }

    [Fact]
    public async Task UpdateUser_UpdatesPassword_OnlyWhenProvided()
    {
        // Arrange
        string oldHash = BCrypt.Net.BCrypt.HashPassword("oldPass");
        var user = new User { Id = 1, Name = "Old", Email = "test@test.com", Role = UserRole.EndUser, PasswordHash = oldHash };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Scenario 1: Update bez hasła (hasło puste)
        var updateDtoNoPass = new UpdateUserDTO
        {
            Name = "NewName",
            Email = "test@test.com",
            Role = "EndUser",
            IsActive = true,
            Password = "" // Puste hasło
        };

        await _controller.UpdateUser(1, updateDtoNoPass);
        var userAfterFirstUpdate = await _context.Users.FindAsync(1);
        userAfterFirstUpdate!.PasswordHash.Should().Be(oldHash); // Hash bez zmian

        // Scenario 2: Update z nowym hasłem
        var updateDtoWithPass = new UpdateUserDTO
        {
            Name = "NewName",
            Email = "test@test.com",
            Role = "EndUser",
            IsActive = true,
            Password = "newPassword123"
        };

        await _controller.UpdateUser(1, updateDtoWithPass);
        var userAfterSecondUpdate = await _context.Users.FindAsync(1);

        // Weryfikujemy, że hash się zmienił i jest poprawny dla nowego hasła
        userAfterSecondUpdate!.PasswordHash.Should().NotBe(oldHash);
        BCrypt.Net.BCrypt.Verify("newPassword123", userAfterSecondUpdate.PasswordHash).Should().BeTrue();
    }
}