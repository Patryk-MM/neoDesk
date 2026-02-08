namespace neoDesk.Server.Services {
    public interface IEmailService {
        public Task SendEmailAsync<T>(string templateFIle, string toEmail, string subject, T model);
    }
}
