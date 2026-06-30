using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Auth.Models;

public class LoginRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}