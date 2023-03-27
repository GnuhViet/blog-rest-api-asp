using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Table("user")]
[Index("user_name", Name = "UQ__user__7C9273C4B24B4E3C", IsUnique = true)]
public partial class user
{
    [Key]
    public int id { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string user_name { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string password { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string? phone_number { get; set; }

    [StringLength(255)]
    public string full_name { get; set; } = null!;

    [StringLength(255)]
    public string? email { get; set; }

    [StringLength(255)]
    public string? avatar { get; set; }

    [Unicode(false)]
    public string? modified_log { get; set; }

    public int? level { get; set; }

    [InverseProperty("create_by")]
    public virtual ICollection<article> articles { get; } = new List<article>();

    [InverseProperty("user")]
    public virtual ICollection<comment> comments { get; } = new List<comment>();
}
