using System.ComponentModel.DataAnnotations;
using web_blog.Entities;

namespace web_blog.Models;

public class ArticleResponseModel
{
    public int ? Id { get; set; }
    public List<Category> Categories { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Thumbnail { get; set; } = null!;
    public string ShortDescription { get; set; } = null!;
    public string Content { get; set; } = null!;
    public DateTime CreateDate { get; set; }
    public int CreateByBlogUserId { get; set; }
}