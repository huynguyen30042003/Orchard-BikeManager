using System.ComponentModel.DataAnnotations;

namespace BikeManagerV3.Auth.Models;

public class UserModel
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(200)]
    public required string FullName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public required string Email { get; set; }

    [Phone]
    [StringLength(20)]
    public required string PhoneNumber { get; set; }

    public string? PasswordHash { get; set; }

    public string? AvatarUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime? LastLoginAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}