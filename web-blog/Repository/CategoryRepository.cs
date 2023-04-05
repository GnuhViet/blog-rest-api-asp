using Microsoft.EntityFrameworkCore;
using web_blog.Context;
using web_blog.Entities;

namespace web_blog.Repository;

public class CategoryRepository
{
    private readonly BlogDbContext _context;

    public CategoryRepository(BlogDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Category>> GetAll()
    {
        return await _context.Categories.ToListAsync();
    }

    public Category Create(Category category)
    {
        _context.Add(category);
        _context.SaveChanges();
        return category;
    }

    public void SetCategory(int articleId, List<int> categoryIds)
    {
        foreach (var id in categoryIds)
        {
            ArticleCategory ac = new ArticleCategory();
            ac.ArticleId = articleId;
            ac.CategoryId = id;
            _context.Database.ExecuteSqlRaw($"INSERT INTO ArticleCategory values ({articleId}, {id})");
        }
    }

    public Category GetCategoryById(int id)
    {
        List<Category> c = _context.Categories.Where(c => c.Id == id).ToList();
        if (c.Count == 0)
        {
            return null;
        }

        return c[0];
    }

    public Category Update(Category category)
    {
        _context.Update(category);
        _context.SaveChanges();
        return category;
    }

    public int Delete(int id)
    {
        Category c = GetCategoryById(id);
        if (c == null)
        {
            return -1;
        }

        _context.Remove(c);
        _context.SaveChanges();
        return id;
    }
}