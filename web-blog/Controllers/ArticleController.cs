using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Filter;
using web_blog.Helper;
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
    private readonly UriService _uriService;

    public ArticleController(ArticleService articleService, IMapper mapper, UriService uriService)
    {
        _articleService = articleService;
        _mapper = mapper;
        _uriService = uriService;
    }

	[AllowAnonymous]
	[HttpGet("category/{categoryId}")]
	public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> Article([FromQuery] PaginationFilter filter, int categoryId) {
		var route = Request.Path.Value;
		var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
		var pagedData = _articleService.GetResponseModel(await _articleService.GetByCategoryPaging(validFilter.PageNumber, validFilter.PageSize, categoryId));
		var totalRecords = await _articleService.TotalRecordAsync();

		var pagedReponse = PaginationHelper.CreatePagedReponse<ArticleResponseModel>(pagedData, validFilter, totalRecords, _uriService, route);
		return Ok(pagedReponse);
	}


	[AllowAnonymous]    	
    [HttpGet("search/{title}")]
    public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> Article([FromQuery] PaginationFilter filter, string title)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = _articleService.GetResponseModel(await _articleService.SearchPaging(validFilter.PageNumber, validFilter.PageSize, title));
        var totalRecords = await _articleService.TotalRecordAsync();
        
        var pagedReponse = PaginationHelper.CreatePagedReponse<ArticleResponseModel>(pagedData, validFilter, totalRecords, _uriService, route);
        return Ok(pagedReponse);
    }

	[AllowAnonymous]    	
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> Article([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = _articleService.GetResponseModel(await _articleService.GetPaging(validFilter.PageNumber, validFilter.PageSize));
        var totalRecords = await _articleService.TotalRecordAsync();
        
        var pagedReponse = PaginationHelper.CreatePagedReponse<ArticleResponseModel>(pagedData, validFilter, totalRecords, _uriService, route);
        return Ok(pagedReponse);
    }

	[AllowAnonymous]
	[HttpGet("{id}")]
    public async Task<ActionResult<ArticleResponseModel>> Article(int id)
    {
        Article article = _articleService.FindById(id);
        if (article == null)
        {
            return new NotFoundObjectResult("not found article with id - " + id);
        }

        List<Article> articles = new List<Article>();
        articles.Add(article);
        
        return _articleService.GetResponseModel(articles)[0];
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