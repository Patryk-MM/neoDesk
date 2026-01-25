using neoDesk.Server.Models;

namespace neoDesk.Server.Helpers {
    public class TicketFilterParams {
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }


        public string? SearchTerm { get; set; }
        public List<Status>? Statuses { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
