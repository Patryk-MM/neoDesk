using Microsoft.EntityFrameworkCore;
using neoDesk.Server.Models;

namespace neoDesk.Server.Data;

public class NeoDeskDbContext : DbContext {
    public NeoDeskDbContext(DbContextOptions<NeoDeskDbContext> options) : base(options) {
    }

    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.CreatedByUser)
            .WithMany(u => u.CreatedTickets)
            .HasForeignKey(t => t.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.AssignedToUser)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssignedToUserId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.User) 
            .WithMany()   
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Ticket)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TicketId)
            .OnDelete(DeleteBehavior.Cascade); 

        // Seed data
        modelBuilder.Entity<User>().HasData(
            new User {
                Id = 1,
                Name = "Administrator",
                Email = "admin@neodesk.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new User {
                Id = 2,
                Name = "Jan Kowalski",
                Email = "jan.kowalski@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = UserRole.EndUser,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new User {
                Id = 3,
                Name = "Anna Nowak",
                Email = "anna.nowak@company.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = UserRole.Technician,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket {
                Id = 1,
                Title = "Problem z drukarką",
                Description = "Drukarka nie drukuje dokumentów",
                CreatedAt = DateTime.Now.AddDays(-2),
                Category = Category.Hardware,
                Status = Status.New,
                CreatedByUserId = 1
            },
            new Ticket {
                Id = 2,
                Title = "Błąd w aplikacji",
                Description = "Aplikacja CRM wyświetla błąd przy logowaniu",
                CreatedAt = DateTime.Now.AddDays(-1),
                Category = Category.Software,
                Status = Status.Assigned,
                CreatedByUserId = 2,
                AssignedToUserId = 3
            }
        );
    }
}