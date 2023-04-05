using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace web_blog.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<BlogUser> _userManager;
    private readonly SignInManager<BlogUser> _signInManager;
    private readonly UserRepository _userRepository;
    private RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountService(UserManager<BlogUser> userManager, SignInManager<BlogUser> signInManager,RoleManager<IdentityRole> roleManager, IConfiguration configuration, UserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _userRepository = userRepository;
    }

    public async Task<bool> SetAdmin(int blogUserId)
    {
        BlogUser user = _userRepository.GetUserById(blogUserId);

        if (user == null)
        {
            return false;
        }

        try
        {

            await _userManager.AddToRoleAsync(user, BlogRoles.Admin);
        }
        catch (Exception e)
        {
            Console.Write(e.Message);
        }
        return true;
    }

    public async Task<IdentityResult> SignUpAsync(SignUpModel model)
    {
        var user = new BlogUser
        {
            FullName = model.FullName,
            UserName = model.Username,
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
        return await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
    }

    public async Task<JwtSecurityToken> GenerateJwtToken(SignInModel model)
    {
        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, model.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var user = await _userManager.FindByNameAsync(model.Username);
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