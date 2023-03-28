using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

public partial class Comment
{
    [Key]
    public long Id { get; set; }

    [Unicode(false)]
    public string Content { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? CreateDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? ModifiedDate { get; set; }

    [Unicode(false)]
    public string? ModifiedLog { get; set; }

    public int CreateByBlogUserId { get; set; }

    public int ArticleId { get; set; }

    [ForeignKey("ArticleId")]
    [InverseProperty("Comments")]
    public virtual Article Article { get; set; } = null!;

    public virtual BlogUser CreateByBlogUser { get; set; } = null!;
}
