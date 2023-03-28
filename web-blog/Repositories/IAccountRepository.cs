using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using web_blog.Models;

namespace web_blog.Repositories;

public interface IAccountRepository
{
    public Task<IdentityResult> SignUpAsync(SignUpModel model);
    public Task<SignInResult> SignInAsync(SignInModel model);
    public Task<JwtSecurityToken> GetJwtToken(SignInModel model);
}