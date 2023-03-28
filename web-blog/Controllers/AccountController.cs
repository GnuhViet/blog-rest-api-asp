using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using web_blog.Models;
using web_blog.Repositories;

namespace web_blog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IAccountRepository _accountRepository;
    

    public AccountController(IAccountRepository accountRepository, RoleManager<IdentityRole> roleManager)
    {
        _accountRepository = accountRepository;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp(SignUpModel signUpModel)
    {
        var result = await _accountRepository.SignUpAsync(signUpModel);

        if (result.Succeeded)
        {
            return Ok(result.Succeeded);
        }

        return Unauthorized(result.Errors);
    }

    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignInModel signInModel)
    {
        var result = await _accountRepository.SignInAsync(signInModel);

        if (!result.Succeeded)
        {
            return Unauthorized("User name or password is invalid");
        }

        var token = await _accountRepository.GetJwtToken(signInModel);
        
        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    }

}