using System.ComponentModel.DataAnnotations;

namespace neoDesk.Server.DTOs;

public class ResetPasswordDTO {
    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;
}