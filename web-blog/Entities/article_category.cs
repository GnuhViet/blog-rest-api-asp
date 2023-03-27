using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Keyless]
[Table("article_category")]
public partial class article_category
{
    public int article_id { get; set; }

    public int category_id { get; set; }

    [ForeignKey("article_id")]
    public virtual article article { get; set; } = null!;

    [ForeignKey("category_id")]
    public virtual category category { get; set; } = null!;
}
