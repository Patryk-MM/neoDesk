using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using neoDesk.Server.DTOs;
using neoDesk.Server.Models;

namespace neoDesk.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase {
    // GET
    [HttpGet(Name = "GetTickets")]
    public IEnumerable<TicketDTO> Get()
    {
        var random = Random.Shared;
        var categories = Enum.GetValues<Category>();
        var statuses = Enum.GetValues<Status>();

        return Enumerable.Range(1, 11).Select(i =>
        {
            var ticket = new Ticket
            {
                Id = i,
                Title = $"Test {i}",
                Description = $"Zgłoszenie testowe nr {i}",
                CreatedAt = DateTime.Now.AddDays(i - 7),
                Category = categories[random.Next(categories.Length)],
                Status = statuses[random.Next(statuses.Length)]
            };

            return new TicketDTO
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                CreatedAt = ticket.CreatedAt.ToString("g"),
                Category = ticket.Category.GetDisplayName(),
                Status = ticket.Status.GetDisplayName()
            };
        });
    }
}