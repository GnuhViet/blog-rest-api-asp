using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;

// using web_blog.Context;
// using web_blog.Entities;

namespace web_blog.Controllers;

//https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-7.0&tabs=visual-studio
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    
    private readonly UserRepository _userRepository;

    public UserController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // GET: api/users
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<UserModel>>> Users()
    // {
    //     return _userRepository.GetUserById()
    // }
    //
    // GET: api/users/5
    // <snippet_GetByID>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<BlogUser>> GetUserById(int id)
    {
        BlogUser user = _userRepository.GetUserById(id);
        if (user == null)
        {
            return new NotFoundObjectResult("not found use with id - " + id);
        }
        return user;
    }
    //
    // // POST: api/users
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // // <snippet_Create>
    // [HttpPost]
    // public async Task<ActionResult<user>> PostTodoItem(user user)
    // {
    //     
    //     _context.users.Add(user);
    //     await _context.SaveChangesAsync();
    //
    //     return new CreatedAtActionResult("actionName","user","api/[controller]",user);
    // }
    // // </snippet_Create>
    //
    //
    // // PUT: api/TodoItems/5
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // // <snippet_Update>
    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutTodoItem(int id, user putUser)
    // {
    //     if (id != putUser.id)
    //     {
    //         return new BadRequestResult();
    //     }
    //
    //     var dbUser = await _context.users.FindAsync(id);
    //     if (dbUser == null)
    //     {
    //         return new NotFoundResult();
    //     }
    //     
    //     // update more, ...
    //     dbUser.full_name = putUser.full_name;
    //     
    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException) when (!UserExist(id))
    //     {
    //         return new NotFoundResult();
    //     }
    //
    //     return new NoContentResult();
    // }
    // // </snippet_Update>
    //
    // // DELETE: api/TodoItems/5
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteTodoItem(int id)
    // {
    //     var user = await _context.users.FindAsync(id);
    //     if (user == null)
    //     {
    //         return new NotFoundResult();
    //     }
    //
    //     _context.users.Remove(user);
    //     await _context.SaveChangesAsync();
    //
    //     return new NoContentResult();
    // }
    //
    //
    // private bool UserExist(long id)
    // {
    //     return _context.users.Any(e => e.id == id);
    // }
}