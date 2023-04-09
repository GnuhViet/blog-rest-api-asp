using web_blog.Context;
using web_blog.Entities;

namespace web_blog.Repository;

public class ArticleCategoryRepository
{
    private readonly BlogDbContext _context;

    public ArticleCategoryRepository(BlogDbContext context)
    {
        _context = context;
    }

    public List<ArticleCategory> GetCategoryList(int articleId)
    {
        return _context.ArticleCategories.Where(x => x.ArticleId == articleId).ToList();
    }
}