using neoDesk.Server.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace neoDesk.Server.DTOs {
    public class CommentDTO {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; } = string.Empty;

        public int TicketId { get; set; }
        public SimpleUserDTO User { get; set; } = null!;
    }
}