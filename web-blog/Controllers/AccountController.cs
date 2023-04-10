using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;
using web_blog.Services;

namespace web_blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    private ImageService _imageService;
    private UserRepository _userRepository;
    private IMapper _mapper;

    public AccountController(IAccountService accountService, UserRepository userRepository, IMapper mapper, ImageService imageService)
    {
        _accountService = accountService;
        _userRepository = userRepository;
        _mapper = mapper;
        _imageService = imageService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("SetAdmin/{blogUserId}")]
    public async Task<IActionResult> SetAdmin(int blogUserId)
    {
        var res = _accountService.SetAdmin(blogUserId);
        if (res.Result == false)
        {
            return BadRequest();
        }

        return Ok();
    }

    [Authorize]
    [HttpGet("UserDetails")]
    public async Task<IActionResult> UserDetails()
    {
        // var  userid= User.Claims.Where(x => x.Type == "id").FirstOrDefault()?.Value;
        var  username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

        if (username == null)
        {
            return Unauthorized();
        }
        
        var blogUser = _userRepository.GetUserByUserName(username);
        var modelUser = _mapper.Map<BlogUser, UserModel>(blogUser);
        return Ok(new { user = modelUser });
    }

    [Authorize]
    [HttpPut("UserDetails")]
    public async Task<IActionResult> UserDetails(UserModel model)
    {
        var username = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;

        if (username != model.Username)
        {
            return BadRequest();
        }

        var oldUser = _userRepository.GetUserByUserName(username);

		
        if (model.Avatar != null)
        {
			if (!model.Avatar.StartsWith("/api/Image/avatar/")) {
				string fileName = await _imageService.SaveImageAsync(model.Avatar, username);
				oldUser.Avatar = "/api/Image/avatar/" + fileName;
			}
        }
        
        oldUser.FullName = model.FullName;
        oldUser.Email = model.Email;
        oldUser.PhoneNumber = model.PhoneNumber;

        _userRepository.UpdateUser(oldUser);
        
        return Ok();
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpModel signUpModel)
    {
        var result = await _accountService.SignUpAsync(signUpModel);

        if (result.Succeeded)
        {
            return Ok(result.Succeeded);
        }

        return Unauthorized(result.Errors);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        var result = await _accountService.SignInAsync(signInModel);

        if (!result.Succeeded)
        {
            return Unauthorized("User name or password is invalid");
        }

        var token = await _accountService.GenerateJwtToken(signInModel);
        
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }
}