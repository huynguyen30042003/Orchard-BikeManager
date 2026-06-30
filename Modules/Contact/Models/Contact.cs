namespace Contact.Models;

public class ContactModel
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsPublic { get; set; }

    public int Status { get; set; }

    public string? ReplyContent { get; set; }

    public DateTime? ReplyDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public class ContactRequest
{
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? Address { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public bool IsPublic { get; set; }
}

public class ContactReplyRequest
{
    public Guid Id { get; set; }

    public string ReplyContent { get; set; } = string.Empty;

    public int Status { get; set; }
}
