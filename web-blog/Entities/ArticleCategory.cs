using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Keyless]
[Table("ArticleCategory")]
public partial class ArticleCategory
{
    public int ArticleId { get; set; }

    public int CategoryId { get; set; }

    [ForeignKey("ArticleId")]
    public virtual Article Article { get; set; } = null!;

    [ForeignKey("CategoryId")]
    public virtual Category Category { get; set; } = null!;
}
