using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_blog.Models;
using web_blog.Services;

namespace web_blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    

    public AccountController(IAccountService accountService, RoleManager<IdentityRole> roleManager)
    {
        _accountService = accountService;
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

        var token = await _accountService.GetJwtToken(signInModel);
        
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

}