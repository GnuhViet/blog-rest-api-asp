using System.ComponentModel.DataAnnotations;

namespace web_blog.Models;

public class UserModel
{
    [Required] public int BlogUserId { get; set; } = -1!;
    public string? Avatar { get; set; }
    [Required] public string? Username { get; set; } = null!;
    [Required] public string FullName { get; set; } = null!;
    [Required] public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}