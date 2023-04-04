using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Repository;

namespace web_blog.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly CategoryRepository _categoryRepository;

    public CategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> Category()
    {
        return await _categoryRepository.GetAll();
    }
    
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategoryById(int id)
    {
        Entities.Category c = _categoryRepository.GetCategoryById(id);
        if (c == null)
        {
            return new NotFoundObjectResult("not found use with id - " + id);
        }

        return c;
    }

    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category c)
    {
        Category res = _categoryRepository.Create(c);
        if (c == null)
        {
            return BadRequest();
        }

        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> PutCategory(int id, Category c)
    {
        Entities.Category old = _categoryRepository.GetCategoryById(id);
        if (old == null)
        {
            return new NotFoundObjectResult("not found use with id - " + id);
        }

        old.Code = c.Code;
        old.Name = c.Name;
        
        Category res = _categoryRepository.Update(old);
        
        return Ok(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Category>> DeleteCategory(int id)
    {
        int res = _categoryRepository.Delete(id);
        if (res == -1)
        {
            return BadRequest();
        }

        return Ok();
    }
}