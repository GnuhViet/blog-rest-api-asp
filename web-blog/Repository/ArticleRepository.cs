using Microsoft.EntityFrameworkCore;
using web_blog.Context;
using web_blog.Entities;

namespace web_blog.Repository;

public class ArticleRepository
{
    private readonly BlogDbContext _context;

    public ArticleRepository(BlogDbContext context)
    {
        _context = context;
    }

    public Article Create(Article article)
    {
        _context.Articles.Add(article);
        _context.SaveChanges();
        return article;
    }

    public async Task<List<Article>> GetAll()
    {
        return await _context.Articles.ToListAsync();
    }

    public Article FindById(int id)
    {
        List<Article> articles = _context.Articles.Where(u => u.Id == id).ToList();
        if (articles.Count == 0)
        {
            return null;
        }
        return articles[0];
    }

    public Article Update(Article article)
    {
        _context.Articles.Update(article);
        _context.SaveChanges();
        return article; 
    }

    public void Delete(Article article)
    {
        _context.Articles.Remove(article);
        _context.SaveChanges();
    }

    public async Task<List<Article>> GetByUserIdAsync(int blogUserId)
    {
        return await _context.Articles.Where(a => a.CreateByBlogUserId == blogUserId).ToListAsync();
    }
}