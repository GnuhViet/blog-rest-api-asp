using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Table("article")]
public partial class article
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    public string title { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? thumbnail { get; set; }

    [Unicode(false)]
    public string? short_description { get; set; }

    [Unicode(false)]
    public string content { get; set; } = null!;

    [Column(TypeName = "date")]
    public DateTime? create_date { get; set; }

    [Column(TypeName = "date")]
    public DateTime? modified_date { get; set; }

    [Unicode(false)]
    public string? modified_log { get; set; }

    public int create_by_id { get; set; }

    [InverseProperty("article")]
    public virtual ICollection<comment> comments { get; } = new List<comment>();

    [ForeignKey("create_by_id")]
    [InverseProperty("articles")]
    public virtual user create_by { get; set; } = null!;
}
