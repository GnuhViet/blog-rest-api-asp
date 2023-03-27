using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

public partial class comment
{
    [Key]
    public long id { get; set; }

    [Unicode(false)]
    public string content { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? create_date { get; set; }

    [Column(TypeName = "date")]
    public DateTime? modified_date { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? create_by_id { get; set; }

    [Unicode(false)]
    public string? modified_log { get; set; }

    public int user_id { get; set; }

    public int article_id { get; set; }

    [ForeignKey("article_id")]
    [InverseProperty("comments")]
    public virtual article article { get; set; } = null!;

    [ForeignKey("user_id")]
    [InverseProperty("comments")]
    public virtual user user { get; set; } = null!;
}
