using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
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
    private readonly IMapper _mapper;

    public AdminController(ArticleService articleService, UserRepository userRepository, IMapper mapper)
    {
        _articleService = articleService;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet("ListUser")]
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

    [HttpGet("ListArticle/{blogUserId}")]
    public async Task<ActionResult<IEnumerable<Article>>> Users(int blogUserId)
    {
        return await _articleService.GetByUserId(blogUserId);
    }
    
}