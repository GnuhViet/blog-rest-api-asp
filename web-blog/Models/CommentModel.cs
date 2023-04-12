using System.ComponentModel.DataAnnotations;

namespace web_blog.Models;

public class CommentModel
{
    [Required] public string Content { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public string FormattedCreateDate
    {
        get { return CreateDate.ToString("yyyy-MM-dd"); }
    }
    public int CreateByBlogUserId { get; set; }
    public string? AuthorAvatar { get; set; }
    public string? AuthorFullName { get; set; }
}