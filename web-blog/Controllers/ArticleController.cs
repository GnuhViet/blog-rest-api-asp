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
		var list = await _articleService.GetByCategoryPaging(validFilter.PageNumber, validFilter.PageSize, categoryId);
        
        return Ok(CreatePagingResponse(validFilter, list, route).Result);
	}


	[AllowAnonymous]    	
    [HttpGet("search/{title}")]
    public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> Article([FromQuery] PaginationFilter filter, string title)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _articleService.SearchPaging(validFilter.PageNumber, validFilter.PageSize, title);
        
        return Ok(CreatePagingResponse(validFilter, list, route).Result);
    }

	[AllowAnonymous]    	
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> Article([FromQuery] PaginationFilter filter)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _articleService.GetPaging(validFilter.PageNumber, validFilter.PageSize);
        
        return Ok(CreatePagingResponse(validFilter, list, route).Result);
    }
    
    [HttpGet("manage")]
    public async Task<ActionResult<IEnumerable<ArticleResponseModel>>> ArticleManage([FromQuery] PaginationFilter filter)
    {
        var username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var list = await _articleService.GetByUserNamePaging(validFilter.PageNumber, validFilter.PageSize, username);
        
        return Ok(CreatePagingResponse(validFilter, list, route).Result);
    }

    private async Task<PagedResponse<List<ArticleResponseModel>>> CreatePagingResponse(PaginationFilter validFilter, List<Article> result, string? route)
    {
        var pagedData = _articleService.GetResponseModel(result);
        var totalRecords = await _articleService.TotalRecordAsync();
        return PaginationHelper.CreatePagedReponse<ArticleResponseModel>(pagedData, validFilter, totalRecords, _uriService, route);
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
    public async Task<ActionResult<Article>> Article(ArticleRequestModel articleRequest)
    {
        var username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
        Article res = _articleService.Create(username,_mapper.Map<ArticleRequestModel, Article>(articleRequest));
        _articleService.SetCategory(res, articleRequest.CategoryIds);
        return Ok(res);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Article(int id, ArticleRequestModel articleRequest)
    {
        IActionResult authorizeRes = AuthorizeUser(id);
        if (authorizeRes != null)
        {
            return authorizeRes;
        }

        Article res =_articleService.Update(_mapper.Map<ArticleRequestModel, Article>(articleRequest));
        _articleService.SetCategory(res, articleRequest.CategoryIds);
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