
using Microsoft.EntityFrameworkCore;
using MimeKit;
using neoDesk.Server.Data;
using neoDesk.Server.Models;

namespace neoDesk.Server.Services {
    public class EmailPollingService : BackgroundService {

        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;

        public EmailPollingService(IServiceProvider services, ILogger<EmailPollingService> logger, IWebHostEnvironment env) {
            _services = services;
            _logger = logger;
            _env = env;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            var inboxPath = Path.Combine(_env.ContentRootPath, "Emails", "Inbox");
            var processedPath = Path.Combine(_env.ContentRootPath, "Emails", "Processed");

            Directory.CreateDirectory(inboxPath);
            Directory.CreateDirectory(processedPath);

            while (!stoppingToken.IsCancellationRequested) {
                try {
                    var files = Directory.GetFiles(inboxPath, "*.eml");

                    if (files.Length > 0) {
                        _logger.LogInformation($"Znaleziono {files.Length} wiadomości.");

                        using (var scope = _services.CreateScope()) {
                            var context = scope.ServiceProvider.GetRequiredService<NeoDeskDbContext>();

                            foreach (string filePath in files) {
                                bool success = await ProcessEmailAsync(filePath, processedPath, context);

                                if (!success) _logger.LogWarning($"Pominięto plik {Path.GetFileName(filePath)} z powodu błędu.");
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Krytyczny błąd w pętli EmailPollingService");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task<bool> ProcessEmailAsync(string filePath, string processedFolder, NeoDeskDbContext context) {
            try {
                var message = await MimeMessage.LoadAsync(filePath);

                var subject = message.Subject ?? "(Brak tematu)";
                var body = message.HtmlBody ?? message.TextBody ?? "";
                var senderEmail = message.From.Mailboxes.FirstOrDefault()?.Address ?? "";

                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == senderEmail);
                var userId = user?.Id ?? 1;

                var ticket = new Ticket {
                    Title = subject,
                    Description = body,
                    Status = Status.New,
                    Category = Category.Software,
                    CreatedAt = DateTime.Now,
                    LastUpdatedAt = DateTime.Now,
                    CreatedByUserId = userId,
                };

                context.Tickets.Add(ticket);

                await context.SaveChangesAsync();

                var fileName = Path.GetFileName(filePath);
                var destPath = Path.Combine(processedFolder, fileName);

                if (File.Exists(destPath)) File.Delete(destPath);
                File.Move(filePath, destPath);

                _logger.LogInformation($"Utworzono zgłoszenie: '{subject}'");
                return true;
            }
            catch (Exception ex) {
                _logger.LogError($"Błąd pliku {filePath}: {ex.Message}");

                context.ChangeTracker.Clear();
                return false;
            }
        }
    }
}
