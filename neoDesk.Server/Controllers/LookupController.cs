using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using neoDesk.Server.Data;
using neoDesk.Server.DTOs;

namespace neoDesk.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly NeoDeskDbContext _context;

        public LookupController(NeoDeskDbContext context)
        {
            _context = context;
        }

        [HttpGet("get_technicians")]
        public IActionResult GetTechnicians()
        {
            var technicians = _context.Users
                .Where(r => r.Role == Models.UserRole.Technician)
                .ToList();

            var dtos = technicians.Select(t => new SimpleUserDTO
            {
                Id = t.Id,
                Name = t.Name,
            });

            return Ok(dtos);
        }
    }
}
