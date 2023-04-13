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
    
    public async Task<List<Article>> GetPaging(int pageNumber, int pageSize)
    {
        return await _context.Articles.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<List<Article>> GetByUserIdPaging(int pageNumber, int pageSize, int blogUserId)
    {
        return await _context.Articles
            .Where(x => x.CreateByBlogUserId == blogUserId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public async Task<List<Article>> SearchPaging(int pageNumber, int pageSize, string title)
    {
        return await _context.Articles
            .Where(x => x.Title.Contains(title))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

	public async Task<List<Article>> GetByCategoryPaging(int pageNumber, int pageSize, int categoryId) {
		return await _context.Articles
			.Join(_context.ArticleCategories, a => a.Id, ac => ac.ArticleId, (a, ac) => new { Article = a, ArticleCategory = ac })
			.Where(ac => ac.ArticleCategory.CategoryId == categoryId)
			.Select(ac => ac.Article)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
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

    public async Task<int> TotalRecordAsync()
    {
        return await _context.Articles.CountAsync();
    }
}