
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

            // Ensure folders exist (Do this ONCE, outside the loop)
            Directory.CreateDirectory(inboxPath);
            Directory.CreateDirectory(processedPath);

            while (!stoppingToken.IsCancellationRequested) {
                try {
                    // Get files
                    var files = Directory.GetFiles(inboxPath, "*.eml");

                    if (files.Length > 0) {
                        _logger.LogInformation($"Znaleziono {files.Length} wiadomości.");

                        using (var scope = _services.CreateScope()) {
                            var context = scope.ServiceProvider.GetRequiredService<NeoDeskDbContext>();

                            foreach (string filePath in files) {
                                // 1. Process One Email
                                bool success = await ProcessEmailAsync(filePath, processedPath, context);

                                // 2. Only if DB save worked, we continue to the next
                                if (!success) _logger.LogWarning($"Pominięto plik {Path.GetFileName(filePath)} z powodu błędu.");
                            }
                            // Note: We save inside the method now to ensure safety
                        }
                    }
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Krytyczny błąd w pętli EmailPollingService");
                }

                // Wait 5 seconds (1s is a bit too aggressive for file polling)
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task<bool> ProcessEmailAsync(string filePath, string processedFolder, NeoDeskDbContext context) {
            try {
                var message = await MimeMessage.LoadAsync(filePath);

                var subject = message.Subject ?? "(Brak tematu)";
                var body = message.HtmlBody ?? message.TextBody ?? "";
                var senderEmail = message.From.Mailboxes.FirstOrDefault()?.Address ?? "";

                // ✅ ASYNC Call
                var user = await context.Users.FirstOrDefaultAsync(u => u.Email == senderEmail);
                var userId = user?.Id ?? 1;

                var ticket = new Ticket {
                    Title = subject,
                    Description = body,
                    Status = Status.New, // Ensure Enum matches your definition
                    Category = Category.Software,
                    CreatedAt = DateTime.UtcNow, // Use UtcNow usually
                    LastUpdatedAt = DateTime.UtcNow,
                    CreatedByUserId = userId,
                };

                context.Tickets.Add(ticket);

                // ✅ SAVE FIRST
                await context.SaveChangesAsync();

                // ✅ MOVE FILE SECOND (Only if save succeeded)
                var fileName = Path.GetFileName(filePath);
                var destPath = Path.Combine(processedFolder, fileName);

                // Check if file exists in destination to prevent crash
                if (File.Exists(destPath)) File.Delete(destPath);
                File.Move(filePath, destPath);

                _logger.LogInformation($"Utworzono zgłoszenie: '{subject}'");
                return true;
            }
            catch (Exception ex) {
                // If DB fails, file stays in Inbox and we try again next loop
                _logger.LogError($"Błąd pliku {filePath}: {ex.Message}");

                // Detach the failed ticket so EF doesn't try to save it again next time
                context.ChangeTracker.Clear();
                return false;
            }
        }
    }
}
