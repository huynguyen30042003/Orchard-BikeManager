using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Auth.Models;

public class UpdateRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public IFormFile? Avatar { get; set; }
}