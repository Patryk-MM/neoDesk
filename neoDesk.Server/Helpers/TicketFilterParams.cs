using neoDesk.Server.Models;

namespace neoDesk.Server.Helpers {
    public class TicketFilterParams {
        public SortOptions? SortBy { get; set; } = SortOptions.Id;
        public SortDirection? SortDir { get; set; } = SortDirection.Asc;


        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; }
        public List<Status>? Statuses { get; set; }
        public List<Category>? Categories { get; set; }
    }

    public enum SortOptions {
        Id,
        Title,
        CreatedAt,
        Status,
        Category
    }
    
    public enum SortDirection {
        Asc,
        Desc
    }
}
