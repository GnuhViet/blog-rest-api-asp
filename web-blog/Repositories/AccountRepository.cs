using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using web_blog.Entities;
using web_blog.Models;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace web_blog.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AccountRepository(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<IdentityResult> SignUpAsync(SignUpModel model)
    {
        var user = new BlogUser
        {
            FullName = model.FullName,
            UserName = model.UserName,
            Email = model.Email
        };
        return await _userManager.CreateAsync(user, model.Password);
    }

    public async Task<string> SignInAsync(SignInModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        if (!result.Succeeded)
        {
            return string.Empty;
        }

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.UserData, model.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(20),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
        );
        
        
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}