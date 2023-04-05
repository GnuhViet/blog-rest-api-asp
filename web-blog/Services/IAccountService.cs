using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using web_blog.Models;

namespace web_blog.Services;

public interface IAccountService
{
    public Task<bool> SetAdmin(int blogUserId);
    public Task<IdentityResult> SignUpAsync(SignUpModel model);
    public Task<SignInResult> SignInAsync(SignInModel model);
    public Task<JwtSecurityToken> GenerateJwtToken(SignInModel model);
}