using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Filter;
using web_blog.Helper;
using web_blog.Models;
using web_blog.Repository;
using web_blog.Services;

namespace web_blog.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly UserRepository _userRepository;
    private readonly CommentRepository _commentRepository;
    private readonly UriService _uriService;
    private readonly IMapper _mapper;

    public AdminController(ArticleService articleService, UserRepository userRepository, IMapper mapper, UriService uriService, CommentRepository commentRepository)
    {
        _articleService = articleService;
        _userRepository = userRepository;
        _mapper = mapper;
        _uriService = uriService;
        _commentRepository = commentRepository;
    }

    [HttpGet("User")]
    public async Task<ActionResult<IEnumerable<UserModel>>> Users()
    {
        List<BlogUser> blogUsers = await _userRepository.GetAll();
        List<UserModel> userModels = new List<UserModel>();
        foreach (var u in blogUsers)
        {
            userModels.Add(_mapper.Map<BlogUser, UserModel>(u));
        }

        return userModels;
    }

    [HttpGet("User/{blogUserId}/Article/")]
    public async Task<ActionResult<IEnumerable<Article>>> Article([FromQuery] PaginationFilter filter, int blogUserId)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await _articleService.GetByUserIdPaging(validFilter.PageNumber, validFilter.PageSize, blogUserId);
        var totalRecords = await _articleService.TotalRecordAsync();
        
        return Ok(PaginationHelper.CreatePagedReponse<Article>(pagedData, validFilter, totalRecords, _uriService, route));
    }
    
    [HttpDelete("User/{blogUserId}/Article/{articleId}")]
    public async Task<ActionResult<IEnumerable<Article>>> Article(int articleId)
    {
        _articleService.Delete(articleId);
        return Ok();
    }
    
    [HttpGet("User/{blogUserId}/Comments/")]
    public async Task<ActionResult<IEnumerable<Comment>>> Comments([FromQuery] PaginationFilter filter, int blogUserId)
    {
        var route = Request.Path.Value;
        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var pagedData = await _commentRepository.GetByUserIdPaging(validFilter.PageNumber, validFilter.PageSize, blogUserId);
        var totalRecords = await _articleService.TotalRecordAsync();
        
        return Ok(PaginationHelper.CreatePagedReponse<Comment>(pagedData, validFilter, totalRecords, _uriService, route));
    }
    
    [HttpDelete("User/{blogUserId}/Comments/{commentId}")]
    public async Task<ActionResult<IEnumerable<Comment>>> Comments(int commentId)
    {
        _commentRepository.Delete(commentId);
        return Ok();
    }
}