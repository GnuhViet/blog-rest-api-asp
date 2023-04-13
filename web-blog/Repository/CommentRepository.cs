using Microsoft.EntityFrameworkCore;
using web_blog.Context;
using web_blog.Entities;

namespace web_blog.Repository;

public class CommentRepository
{
    private readonly BlogDbContext _context;

    public CommentRepository(BlogDbContext context)
    {
        _context = context;
    }

    public void Create(Comment cmt)
    {
        _context.Comments.Add(cmt);
        _context.SaveChanges();
    }

    public void DeleteByArticleId(int articleId)
    {
        var cmts = _context.Comments.Where(x => x.ArticleId == articleId);
        _context.Comments.RemoveRange(cmts);
        _context.SaveChanges();
    }

    public async Task<List<Comment>> GetByArticle(int articleId)
    {
        return await _context.Comments.Where(x => x.ArticleId == articleId).ToListAsync();
    }

    public async Task<List<Comment>> GetByUserIdPaging(int pageNumber, int pageSize, int blogUserId)
    {
        return await _context.Comments
            .Where(x => x.CreateByBlogUserId == blogUserId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    
    public void Delete(int commentId)
    {
        var cmt = _context.Comments.Where(x => x.Id == commentId).ToList();
        if (cmt.Count > 0)
        {
            _context.Comments.Remove(cmt[0]);
            _context.SaveChanges();
        }
        
    }
}