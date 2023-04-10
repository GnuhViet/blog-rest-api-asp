using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
    [JsonIgnore]
    public DateTime CreateDate { get; set; }
    public string FormattedCreateDate
    {
        get { return CreateDate.ToString("yyyy-MM-dd"); }
    }
    public int CreateByBlogUserId { get; set; }
    public string AuthorName { get; set; } = null!;
}