using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;
using web_blog.Services;

namespace web_blog.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ArticleController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly IMapper _mapper;

    public ArticleController(ArticleService articleService, IMapper mapper)
    {
        _articleService = articleService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Article>>> Article()
    {
        return await _articleService.GetAll();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Article>> Article(int id)
    {
        Article article = _articleService.FindById(id);
        if (article == null)
        {
            return new NotFoundObjectResult("not found article with id - " + id);
        }
        return article;
    }
    
    [HttpPost]
    public async Task<ActionResult<Article>> Article(ArticleModel article)
    {
        var  username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
        Article res = _articleService.Create(username,_mapper.Map<ArticleModel, Article>(article));
        _articleService.SetCategory(res, article.CategoryIds);
        return Ok(res);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Article(int id, ArticleModel article)
    {
        IActionResult authorizeRes = AuthorizeUser(id);
        if (authorizeRes != null)
        {
            return authorizeRes;
        }

        Article res =_articleService.Update(_mapper.Map<ArticleModel, Article>(article));
        
        return Ok(res);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(int id)
    {
        IActionResult authorizeRes = AuthorizeUser(id);
        if (authorizeRes != null)
        {
            return authorizeRes;
        }
        
        _articleService.Delete(id);
        
        return Ok();
    }

    private IActionResult AuthorizeUser(int articleId)
    {
        Article dbArticle = _articleService.FindById(articleId);
        if (dbArticle == null)
        {
            return new NotFoundObjectResult("not found article with id - " + articleId);
        }
        var  username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

        if (_articleService.IsCreateByUser(username, dbArticle))
        {
            return null;
        }

        return new UnauthorizedResult();
    }
}