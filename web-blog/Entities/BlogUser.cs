using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace web_blog.Entities;

[Table("BlogUser")]
[Index("BlogUserId", Name = "UQ__BlogUser__0C12253C97210ECC", IsUnique = true)]
public partial class BlogUser : IdentityUser
{
    public int BlogUserId { get; set; }

    [StringLength(255)]
    public string FullName { get; set; } = null!;

    [StringLength(255)]
    public string? Avatar { get; set; }

    [Unicode(false)]
    public string? ModifiedLog { get; set; }

    public virtual ICollection<Article> Articles { get; } = new List<Article>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();
}
