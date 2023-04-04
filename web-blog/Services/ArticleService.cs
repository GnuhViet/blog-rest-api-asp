using web_blog.Entities;
using web_blog.Repository;

namespace web_blog.Services;

public class ArticleService
{
    private readonly ArticleRepository _articleRepository;
    private readonly UserRepository _userRepository;

    public ArticleService(ArticleRepository articleRepository, UserRepository userRepository)
    {
        _articleRepository = articleRepository;
        _userRepository = userRepository;
    }

    public Article Create(string username, Article article)
    {
        var blogUser = _userRepository.GetUserByUserName(username);
        article.CreateByBlogUserId = blogUser.BlogUserId;
        article.CreateDate = DateTime.Now;
        return _articleRepository.Create(article);
    }

    public async Task<List<Article>> GetAll()
    {
        return await _articleRepository.GetAll();
    }

    public async Task<List<Article>> GetByUserId(int blogUserId)
    {
        return await _articleRepository.GetByUserIdAsync(blogUserId);
    }

    public Article FindById(int id)
    {
        return _articleRepository.FindById(id);
    }

    public Article Update(Article article)
    {
        Article old = _articleRepository.FindById(article.Id);

        old.Title = article.Title;
        old.Content = article.Content;
        old.ShortDescription = article.ShortDescription;
        old.Thumbnail = article.Thumbnail;
        old.ModifiedDate = DateTime.Now;

        return _articleRepository.Update(old);
    }

    public bool IsCreateByUser(string username, Article article)
    {
        var blogUser = _userRepository.GetUserByUserName(username);
        if (article.CreateByBlogUserId != blogUser.BlogUserId)
        {
            return false;
        }

        return true;
    }

    public void Delete(int id)
    {
        Article article = _articleRepository.FindById(id);
        _articleRepository.Delete(article);
    }
}