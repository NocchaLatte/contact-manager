using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

/// <summary>
/// Request payload for creating a new contact.
/// </summary>
public class ContactCreateDto
{
    /// <summary>
    /// The name of the contact.
    /// </summary>
    /// <example>John Doe</example>
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the contact.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Required, MaxLength(200), EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the contact.
    /// </summary>
    /// <example>123-456-7890</example>
    [MaxLength(30)]
    public string? Phone { get; set; }

    /// <summary>
    /// Notes or additional information about the contact.
    /// </summary>
    /// <example>Met at career fair.</example>
    [MaxLength(2000)]
    public string? Note { get; set; }
}

/// <summary>
/// Request payload for updating an existing contact.
/// </summary>
public class ContactUpdateDto
{

    /// <summary>
    /// The unique identifier of the contact to update.
    /// </summary>
    /// <example>1</example>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// The name of the contact.
    /// </summary>
    /// <example>John Doe</example>
    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the contact.
    /// </summary>
    /// <example>john.doe@example.com</example>
    [Required, MaxLength(200), EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The phone number of the contact.
    /// </summary>
    /// <example>123-456-7890</example>
    [MaxLength(30)]
    public string? Phone { get; set; }

    /// <summary>
    /// Notes or additional information about the contact.
    /// </summary>
    /// <example>Met at career fair.</example>
    [MaxLength(2000)]
    public string? Note { get; set; }
}
