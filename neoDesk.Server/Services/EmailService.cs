using MimeKit;
using RazorLight;
using System.Runtime.CompilerServices;

namespace neoDesk.Server.Services {
    public class EmailService : IEmailService {

        private readonly IWebHostEnvironment _env;
        private readonly RazorLightEngine _engine;
        public EmailService(IWebHostEnvironment env) {
            _env = env;
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Path.Combine(_env.ContentRootPath, "Emails\\Templates"))
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task SendEmailAsync<T>(string templateFile, string toEmail, string subject, T model) {
            var message = new MimeMessage();
        
            message.From.Add(new MailboxAddress("Neodesk Service", "notifications@neodesk.com"));
            message.To.Add(new MailboxAddress("", toEmail));

            message.Subject = subject;

            message.Body = new TextPart("html") {
                Text = await _engine.CompileRenderAsync(templateFile, model)
            };

            var folder = Path.Combine(_env.ContentRootPath, "Emails\\SentEmails");
            Directory.CreateDirectory(folder);
            var fileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{toEmail}.eml";

            using (var stream = File.Create(Path.Combine(folder, fileName))) {
                await message.WriteToAsync(stream);
            }

        }

    }
}
