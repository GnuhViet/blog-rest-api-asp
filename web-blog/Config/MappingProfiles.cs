using AutoMapper;
using web_blog.Entities;
using web_blog.Models;

namespace web_blog.Config;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        // source, des
        CreateMap<BlogUser, UserModel>();
        CreateMap<UserModel, BlogUser>();
        CreateMap<BlogUser, BlogUser>();
        CreateMap<ArticleRequestModel, Article>();
        CreateMap<Article, ArticleResponseModel>();
        
        CreateMap<CommentModel, Comment>();
        CreateMap<Comment, CommentModel>();
    }
}