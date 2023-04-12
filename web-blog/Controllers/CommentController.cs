using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;
using web_blog.Services;

namespace web_blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly CommentRepository _commentRepository;
    private readonly UserRepository _userRepository;
    private readonly ArticleService _articleService;
    private readonly IMapper _mapper;

    public CommentController(CommentRepository commentRepository, UserRepository userRepository, ArticleService articleService, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _userRepository = userRepository;
        _articleService = articleService;
        _mapper = mapper;
    }
    
    [Authorize]
    [HttpPost("{articleId}")]
    public async Task<IActionResult> Comment(int articleId, CommentModel model)
    {
        Article article = _articleService.FindById(articleId);
        if (article == null)
        {
            return BadRequest("Not found article with id - " + articleId);
        }
        
        var username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

        var user = _userRepository.GetUserByUserName(username);
        if (user == null)
        {
            return BadRequest();
        }

        Comment cmt = _mapper.Map<CommentModel, Comment>(model);
        cmt.ArticleId = articleId;
        cmt.CreateByBlogUserId = user.BlogUserId;
        cmt.CreateDate = DateTime.Now.Date;

        _commentRepository.Create(cmt);
        
        return Ok();
    }
    
    [AllowAnonymous]
    [HttpGet("{articleId}")]
    public async Task<ActionResult<IEnumerable<CommentModel>>> ArticleComment(int articleId)
    {
        Article article = _articleService.FindById(articleId);
        if (article == null)
        {
            return BadRequest("Not found article with id - " + articleId);
        }

        List<CommentModel> res = new List<CommentModel>();
        foreach (var cmt in _commentRepository.GetByArticle(articleId).Result)
        {
            var cmtModel = _mapper.Map<Entities.Comment, CommentModel>(cmt);
            var user = _userRepository.GetUserById(cmtModel.CreateByBlogUserId);
            cmtModel.AuthorFullName = user.FullName;
            cmtModel.AuthorAvatar = user.Avatar;
            res.Add(cmtModel);
        }

        return Ok(res);
    }
}