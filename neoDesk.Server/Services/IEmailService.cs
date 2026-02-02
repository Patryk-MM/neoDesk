namespace neoDesk.Server.Services {
    public interface IEmailService {
        public Task SendEmailAsync(string toEmail);
    }
}
