using web_blog.Context;
using web_blog.Entities;
using web_blog.Models;

namespace web_blog.Repository;

public class UserRepository
{
    private readonly BlogDbContext _context;

    public UserRepository(BlogDbContext context)
    {
        _context = context;
    }
    
    public BlogUser GetUserById(int id)
    {
        List<BlogUser> users = _context.Users.Where(u => u.BlogUserId == id).ToList();
        if (users.Count == 0)
        {
            return null;
        }
        return users[0];
    }

    public BlogUser GetUserByUserName(string username)
    {
        List<BlogUser> users = _context.Users.Where(u => u.UserName == username).ToList();
        if (users.Count == 0)
        {
            return null;
        }
        return users[0]; 
    }

    public void UpdateUser(BlogUser user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
    
    // public 
}