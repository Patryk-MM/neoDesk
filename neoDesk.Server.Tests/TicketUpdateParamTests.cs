using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neoDesk.Server.Controllers;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;
using neoDesk.Server.Models;

namespace neoDesk.Server.Tests.Controllers;

public class TicketUpdateParamTests
{
    private readonly NeoDeskDbContext _context;
    private readonly TicketController _controller;

    public TicketUpdateParamTests()
    {
        var options = new DbContextOptionsBuilder<NeoDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NeoDeskDbContext(options);
        _controller = new TicketController(_context);
    }

    private void SimulateLoggedInUser(int userId, string role)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    // 1. TEST TYTUŁU I OPISU (Dostępne dla twórcy)
    [Fact]
    public async Task Put_UpdatesTitleAndDescription_WhenUserIsCreator()
    {
        // Arrange
        var creator = new User { Id = 10, Name = "Creator", Email = "c@c.com", Role = UserRole.EndUser, PasswordHash = "x" };
        _context.Users.Add(creator);

        var ticket = new Ticket
        {
            Id = 1,
            Title = "Stary Tytuł",
            Description = "Stary Opis",
            CreatedByUserId = 10,
            CreatedByUser = creator,
            Status = Status.New,
            Category = Category.Hardware
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(10, "EndUser"); // Logujemy się jako twórca

        var updateDto = new UpdateTicketDTO
        {
            Title = "Nowy Tytuł",
            Description = "Nowy Opis",
            Status = Status.New, // Bez zmian
            Category = Category.Hardware // Bez zmian
        };

        // Act
        var result = await _controller.Put(1, updateDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        var dbTicket = await _context.Tickets.FindAsync(1);
        dbTicket!.Title.Should().Be("Nowy Tytuł");
        dbTicket.Description.Should().Be("Nowy Opis");
    }

    // 2. TEST KATEGORII (Dostępne np. dla przypisanego technika)
    [Fact]
    public async Task Put_UpdatesCategory_WhenUserIsAssignedTechnician()
    {
        // Arrange
        var tech = new User { Id = 5, Name = "Tech", Role = UserRole.Technician, Email = "t@t.com", PasswordHash = "x" };
        var creator = new User { Id = 1, Name = "User", Role = UserRole.EndUser, Email = "u@u.com", PasswordHash = "x" };
        _context.Users.AddRange(tech, creator);

        var ticket = new Ticket
        {
            Id = 1,
            Title = "T",
            Description = "D",
            Category = Category.Hardware, // Stara kategoria
            CreatedByUserId = 1,
            AssignedToUserId = 5, // Przypisany do testowanego technika
            CreatedByUser = creator
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(5, "Technician");

        var updateDto = new UpdateTicketDTO
        {
            Title = "T",
            Description = "D",
            Status = Status.New,
            Category = Category.Software // Zmieniamy na Software
        };

        // Act
        await _controller.Put(1, updateDto);

        // Assert
        var dbTicket = await _context.Tickets.FindAsync(1);
        dbTicket!.Category.Should().Be(Category.Software);
    }

    // 3. TEST STATUSU (Dostępne dla technika/admina)
    [Fact]
    public async Task Put_UpdatesStatus_WhenUserIsAdmin()
    {
        // Arrange
        var ticket = new Ticket
        {
            Id = 1,
            Title = "T",
            Description = "D",
            Status = Status.New,
            CreatedByUserId = 99 // Ktoś inny
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(1, "Admin"); // Admin może wszystko

        var updateDto = new UpdateTicketDTO
        {
            Title = "T",
            Description = "D",
            Category = Category.Hardware,
            Status = Status.Solved // Zmieniamy na Solved
        };

        // Act
        await _controller.Put(1, updateDto);

        // Assert
        var dbTicket = await _context.Tickets.FindAsync(1);
        dbTicket!.Status.Should().Be(Status.Solved);
    }

    // 4. TEST DATY ZGŁOSZENIA (Tylko Admin) - Pozytywny
    [Fact]
    public async Task Put_UpdatesCreatedAt_WhenUserIsAdmin()
    {
        // Arrange
        var originalDate = DateTime.Now.AddDays(-10);
        var newDate = DateTime.Now.AddDays(-5);

        var ticket = new Ticket
        {
            Id = 1,
            Title = "T",
            Description = "D",
            CreatedByUserId = 99,
            CreatedAt = originalDate
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(1, "Admin");

        var updateDto = new UpdateTicketDTO
        {
            Title = "T",
            Description = "D",
            Category = Category.Hardware,
            Status = Status.New,
            CreatedAt = newDate // Admin próbuje zmienić datę
        };

        // Act
        await _controller.Put(1, updateDto);

        // Assert
        var dbTicket = await _context.Tickets.FindAsync(1);
        // Porównujemy z tolerancją 1 sekundy (baza może zaokrąglić milisekundy)
        dbTicket!.CreatedAt.Should().BeCloseTo(newDate, TimeSpan.FromSeconds(1));
    }

    // 5. TEST DATY ZGŁOSZENIA (Zwykły user) - Negatywny (Ignorowanie zmiany)
    [Fact]
    public async Task Put_DoesNotUpdateCreatedAt_WhenUserIsNotAdmin()
    {
        // Arrange
        var originalDate = DateTime.Now.AddDays(-10);
        var fakeNewDate = DateTime.Now.AddDays(-1); // Data, którą user próbuje "wstrzyknąć"

        var creator = new User { Id = 10, Role = UserRole.EndUser, Name = "U", Email = "e", PasswordHash = "x" };
        _context.Users.Add(creator);

        var ticket = new Ticket
        {
            Id = 1,
            Title = "T",
            Description = "D",
            CreatedByUserId = 10,
            CreatedByUser = creator,
            CreatedAt = originalDate
        };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(10, "EndUser");

        var updateDto = new UpdateTicketDTO
        {
            Title = "T",
            Description = "D",
            Category = Category.Hardware,
            Status = Status.New,
            CreatedAt = fakeNewDate // Próba oszustwa daty
        };

        // Act
        await _controller.Put(1, updateDto);

        // Assert
        var dbTicket = await _context.Tickets.FindAsync(1);

        // Data powinna pozostać STARA (originalDate), system powinien zignorować pole CreatedAt w DTO
        // ponieważ w kontrolerze jest if (userRole == "Admin" && ...)
        dbTicket!.CreatedAt.Should().Be(originalDate);
        dbTicket.CreatedAt.Should().NotBe(fakeNewDate);
    }
}