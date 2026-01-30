using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace neoDesk.Server.Models {
    public class Comment {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; } = string.Empty;

        public int TicketId { get; set;}
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; } = null!;
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;
    }
}
