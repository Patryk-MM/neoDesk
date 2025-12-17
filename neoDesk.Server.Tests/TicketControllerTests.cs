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

public class TicketControllerTests
{
    private readonly NeoDeskDbContext _context;
    private readonly TicketController _controller;

    public TicketControllerTests()
    {
        var options = new DbContextOptionsBuilder<NeoDeskDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new NeoDeskDbContext(options);
        _controller = new TicketController(_context);
    }

    // Helper do symulacji zalogowanego użytkownika
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

    [Fact]
    public async Task Get_ReturnsAllTickets()
    {
        // Arrange
        SimulateLoggedInUser(1, "Admin");

        var user = new User { Id = 1, Name = "Admin", Email = "a@a.com", PasswordHash = "x", Role = UserRole.Admin };
        _context.Users.Add(user);

        _context.Tickets.Add(new Ticket { Title = "T1", Description = "D1", CreatedByUserId = 1, CreatedByUser = user });
        _context.Tickets.Add(new Ticket { Title = "T2", Description = "D2", CreatedByUserId = 1, CreatedByUser = user });
        await _context.SaveChangesAsync();

        // Act
        var result = await _controller.Get();

        // Assert
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        var tickets = okResult!.Value as IEnumerable<TicketDTO>;
        tickets.Should().HaveCount(2);
    }

    [Fact]
    public async Task Put_UpdatesTicket_WhenUserIsAdmin()
    {
        // Arrange - Admin edytuje ticket stworzony przez kogoś innego
        var creator = new User { Id = 2, Name = "User", Email = "u@u.com", PasswordHash = "x", Role = UserRole.EndUser };
        _context.Users.Add(creator);
        var ticket = new Ticket { Id = 1, Title = "Old Title", Description = "Desc", CreatedByUserId = 2, CreatedByUser = creator };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(99, "Admin"); // ID 99 to Admin

        var updateDto = new UpdateTicketDTO { Title = "New Title", Description = "New Desc", Status = Status.New, Category = Category.Software };

        // Act
        var result = await _controller.Put(1, updateDto);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        var dbTicket = await _context.Tickets.FindAsync(1);
        dbTicket!.Title.Should().Be("New Title");
    }

    [Fact]
    public async Task Put_ReturnsForbid_WhenUserIsNotOwnerAndNotAdmin()
    {
        // Arrange - Zwykły user próbuje edytować cudzy ticket
        var creator = new User { Id = 2, Name = "Creator", Email = "c@c.com", PasswordHash = "x", Role = UserRole.EndUser };
        _context.Users.Add(creator);
        var ticket = new Ticket { Id = 1, Title = "Title", Description = "Desc", CreatedByUserId = 2, CreatedByUser = creator };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(3, "EndUser"); // ID 3 to Hacker (inny user)

        var updateDto = new UpdateTicketDTO { Title = "Hacked", Description = "Hacked", Status = Status.New };

        // Act
        var result = await _controller.Put(1, updateDto);

        // Assert
        result.Should().BeOfType<ForbidResult>(); // Oczekujemy 403 Forbidden
    }

    [Fact]
    public async Task AssignTicket_AssignsUser_WhenRoleIsTechnician()
    {
        // Arrange
        var technician = new User { Id = 5, Name = "Tech", Email = "t@t.com", PasswordHash = "x", Role = UserRole.Technician, IsActive = true };
        _context.Users.Add(technician);
        var ticket = new Ticket { Id = 1, Title = "T", Description = "D", CreatedByUserId = 5, CreatedByUser = technician };
        _context.Tickets.Add(ticket);
        await _context.SaveChangesAsync();

        SimulateLoggedInUser(1, "Admin"); // Tylko admin/technik może przypisywać

        var assignDto = new AssignTicketDTO { TicketId = 1, AssignedToUserId = 5 };

        // Act
        var result = await _controller.AssignTicket(1, assignDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();

        var dbTicket = await _context.Tickets.FindAsync(1);
        dbTicket!.AssignedToUserId.Should().Be(5);
    }


    [Fact]
    public async Task GetMyTickets_ReturnsOnlyTicketsRelatedToUser()
    {
        // Arrange
        int myId = 5;
        int otherId = 99;

        var me = new User { Id = myId, Name = "Me", Email = "me@me.com", PasswordHash = "x", Role = UserRole.EndUser };
        var other = new User { Id = otherId, Name = "Other", Email = "o@o.com", PasswordHash = "x", Role = UserRole.EndUser };
        _context.Users.AddRange(me, other);

        // Ticket 1: Stworzony przeze mnie
        _context.Tickets.Add(new Ticket { Title = "My Ticket", Description = "D", CreatedByUserId = myId, CreatedByUser = me });

        // Ticket 2: Przypisany do mnie (np. jestem technikiem)
        _context.Tickets.Add(new Ticket { Title = "Assigned To Me", Description = "D", CreatedByUserId = otherId, AssignedToUserId = myId, CreatedByUser = other });

        // Ticket 3: Cudzy ticket (nie powinienem go widzieć)
        _context.Tickets.Add(new Ticket { Title = "Other Ticket", Description = "D", CreatedByUserId = otherId, CreatedByUser = other });

        await _context.SaveChangesAsync();

        // Symulujemy zalogowanie jako "Ja" (ID: 5)
        SimulateLoggedInUser(myId, "EndUser");

        // Act
        var result = await _controller.GetMyTickets();

        // Assert
        var okResult = result.Result as OkObjectResult;
        var tickets = okResult!.Value as IEnumerable<TicketDTO>;

        tickets.Should().HaveCount(2);
        tickets.Should().Contain(t => t.Title == "My Ticket");
        tickets.Should().Contain(t => t.Title == "Assigned To Me");
        tickets.Should().NotContain(t => t.Title == "Other Ticket");
    }
}