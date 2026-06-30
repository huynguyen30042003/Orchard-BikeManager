using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Auth.Models;

public class RegisterRequest
{
    [Required]
    [StringLength(200)]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(50)]
    public required string Username { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }

}