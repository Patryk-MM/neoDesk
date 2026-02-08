using neoDesk.Server.Models;

namespace neoDesk.Server.Emails.Models {
    public class TicketEmailModel {
        public required Ticket Ticket { get; set; }
        public string TicketUrl => $"https://localhost:4200/ticket/{Ticket.Id}";
    }
}
