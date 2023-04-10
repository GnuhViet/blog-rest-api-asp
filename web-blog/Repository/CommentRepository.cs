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