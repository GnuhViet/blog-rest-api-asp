using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using web_blog.Entities;

namespace web_blog.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<article> articles { get; set; }

    public virtual DbSet<article_category> article_categories { get; set; }

    public virtual DbSet<category> categories { get; set; }

    public virtual DbSet<comment> comments { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=GNUH\\SQLSERVER;Initial Catalog=web_blog;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<article>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__article__3213E83F41BADF17");

            entity.HasOne(d => d.create_by).WithMany(p => p.articles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_create_article");
        });

        modelBuilder.Entity<article_category>(entity =>
        {
            entity.HasOne(d => d.article).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_article_category_article");

            entity.HasOne(d => d.category).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_article_category_category");
        });

        modelBuilder.Entity<category>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__category__3213E83FA3023D8A");
        });

        modelBuilder.Entity<comment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__comments__3213E83F082D4FA2");

            entity.HasOne(d => d.article).WithMany(p => p.comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_article");

            entity.HasOne(d => d.user).WithMany(p => p.comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_user");
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("PK__user__3213E83FB56409E3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
