using Microsoft.AspNetCore.Identity;
using web_blog.Models;

namespace web_blog.Repositories;

public interface IAccountRepository
{
    public Task<IdentityResult> SignUpAsync(SignUpModel model);
    public Task<string> SignInAsync(SignInModel model);
}