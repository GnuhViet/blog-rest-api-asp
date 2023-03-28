using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Table("Article")]
public partial class Article
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string Title { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? Thumbnail { get; set; }

    [Unicode(false)]
    public string? ShortDescription { get; set; }

    [Unicode(false)]
    public string Content { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ModifiedDate { get; set; }

    [Unicode(false)]
    public string? ModifiedLog { get; set; }

    public int CreateByBlogUserId { get; set; }

    [InverseProperty("Article")]
    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual BlogUser CreateByBlogUser { get; set; } = null!;
}
