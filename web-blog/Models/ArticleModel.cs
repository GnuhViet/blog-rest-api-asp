using System.ComponentModel.DataAnnotations;

namespace web_blog.Models;

public class ArticleModel
{
    public int ? Id { get; set; }
    [Required] public List<string> CategoryIds { get; set; } = null!;
    [Required] public string Title { get; set; } = null!;
    [Required] public string Thumbnail { get; set; } = null!;
    [Required] public string ShortDescription { get; set; } = null!;
    [Required] public string Content { get; set; } = null!;
}