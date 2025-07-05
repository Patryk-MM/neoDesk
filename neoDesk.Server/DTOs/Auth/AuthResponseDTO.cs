namespace neoDesk.Server.DTOs.Auth;

public class AuthResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public UserDTO User { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}