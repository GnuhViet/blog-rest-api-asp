using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using web_blog.Entities;

namespace web_blog.Context;

public partial class BlogDbContext : IdentityDbContext<BlogUser>
{
    public BlogDbContext()
    {
    }

    public BlogDbContext(DbContextOptions<BlogDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }

    public virtual DbSet<BlogUser> BlogUsers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

//     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//         => optionsBuilder.UseSqlServer("Data Source=GNUH\\SQLSERVER;Initial Catalog=web_blog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        this.SeedRoles(modelBuilder);
        this.SeedCategory(modelBuilder);
        this.SeedAdminUser(modelBuilder);
        this.SeedArticle(modelBuilder);
        
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Article__3214EC0758F0FCCC");

            entity.HasOne(d => d.CreateByBlogUser).WithMany(p => p.Articles)
                .HasPrincipalKey(p => p.BlogUserId)
                .HasForeignKey(d => d.CreateByBlogUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_create_article");
            
            entity.Property(e => e.Title).HasColumnType("nvarchar(max)"); 
            entity.Property(e => e.ShortDescription).HasColumnType("nvarchar(max)"); 
            entity.Property(e => e.Content).HasColumnType("nvarchar(max)"); 
        });

        modelBuilder.Entity<ArticleCategory>(entity =>
        {
            entity.HasOne(d => d.Article).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_article_category_article");

            entity.HasOne(d => d.Category).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_article_category_category");
        });

        modelBuilder.Entity<BlogUser>(entity =>
        {
            // entity.HasKey(e => e.Id).HasName("PK__BlogUser__3214EC07958E85BC");

            entity.Property(e => e.BlogUserId).ValueGeneratedOnAdd();
            entity.Property(e => e.FullName).HasColumnType("nvarchar(max)"); 
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3214EC07980E2E92");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Comments__3214EC074A6A8E09");

            entity.HasOne(d => d.Article).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_article");

            entity.HasOne(d => d.CreateByBlogUser).WithMany(p => p.Comments)
                .HasPrincipalKey(p => p.BlogUserId)
                .HasForeignKey(d => d.CreateByBlogUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_user");
            entity.Property(e => e.Content).HasColumnType("nvarchar(max)"); 
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<IdentityRole>().HasData(DbSeeds.s_role);
    }

    private void SeedCategory(ModelBuilder builder)
    {
        builder.Entity<Category>().HasData(DbSeeds.s_category);
    }

    private void SeedAdminUser(ModelBuilder builder)
    {
        String adminId = Guid.NewGuid().ToString();
        
        var hasher = new PasswordHasher<BlogUser>();
        builder.Entity<BlogUser>().HasData(new BlogUser
        {
            Id = adminId,
            BlogUserId = -1,
            UserName = "admin",
            FullName = "Admin Nguyen",
            NormalizedUserName = "admin",
            Email = "admin@admin.fake",
            NormalizedEmail = "admin@admin.fake",
            EmailConfirmed = true,
            PasswordHash = hasher.HashPassword(null, "Admin123@"),
            SecurityStamp = string.Empty
        });
        
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = DbSeeds.s_role[0].Id, 
                UserId = adminId
            }, 
            new IdentityUserRole<string>
            {
                RoleId = DbSeeds.s_role[1].Id, 
                UserId = adminId
            }
        );
    }
    
    private void SeedArticle(ModelBuilder builder)
    {
        builder.Entity<Article>().HasData(DbSeeds.s_article);
    }
}