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
    private RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountRepository(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
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
        
        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return result;
        }
        
        await _userManager.AddToRoleAsync(user, BlogRoles.User);

        return result;
    }

    public async Task<SignInResult> SignInAsync(SignInModel model)
    {
        return await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
    }

    public async Task<JwtSecurityToken> GetJwtToken(SignInModel model)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, model.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var user = await _userManager.FindByNameAsync(model.UserName);
        var userRoles = await _userManager.GetRolesAsync(user);

        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        return GetToken(authClaims);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
        );
        
        return token;
    }
}