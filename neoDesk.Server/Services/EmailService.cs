using MimeKit;
using System.Runtime.CompilerServices;

namespace neoDesk.Server.Services {
    public class EmailService : IEmailService {

        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public EmailService(IWebHostEnvironment env, IConfiguration config) {
            _config = config;
            _env = env;
        }

        public async Task SendEmailAsync(string toEmail) {
            var message = new MimeMessage();
        
            message.From.Add(new MailboxAddress("Neodesk Service", "notifications@neodesk.com"));
            message.To.Add(new MailboxAddress("", toEmail));

            message.Subject = "Test message";

            message.Body = new TextPart("html") { 
                Text = """
                
                Dear user,

                this is a test message from Neodesk.
                
                """ };

            var folder = Path.Combine(_env.ContentRootPath, "SentEmails");
            Directory.CreateDirectory(folder);
            var fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{toEmail}.eml";

            using (var stream = File.Create(Path.Combine(folder, fileName))) {
                await message.WriteToAsync(stream);
            }

        }

    }
}
