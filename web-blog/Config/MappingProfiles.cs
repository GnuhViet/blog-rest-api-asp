using AutoMapper;
using web_blog.Entities;
using web_blog.Models;

namespace web_blog.Config;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<BlogUser, UserModel>();
        CreateMap<UserModel, BlogUser>();
        CreateMap<BlogUser, BlogUser>();
    }
}