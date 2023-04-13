using AutoMapper;
using web_blog.Entities;
using web_blog.Models;
using web_blog.Repository;

namespace web_blog.Services;

public class ArticleService
{
    private readonly ArticleRepository _articleRepository;
    private readonly UserRepository _userRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ArticleCategoryRepository _articleCategoryRepository;
    private readonly CommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public ArticleService(ArticleRepository articleRepository, UserRepository userRepository, CategoryRepository categoryRepository, IMapper mapper, ArticleCategoryRepository articleCategoryRepository, CommentRepository commentRepository)
    {
        _articleRepository = articleRepository;
        _userRepository = userRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _articleCategoryRepository = articleCategoryRepository;
        _commentRepository = commentRepository;
    }

    public void SetCategory(Article article, List<int> categoryIds)
    {
        _articleCategoryRepository.DeleteArticleCategory(article.Id);
        _categoryRepository.SetCategory(article.Id, categoryIds);
    }

    public Article Create(string username, Article article)
    {
        var blogUser = _userRepository.GetUserByUserName(username);
        article.CreateByBlogUserId = blogUser.BlogUserId;
        article.CreateDate = DateTime.Now.Date;
        return _articleRepository.Create(article);
    }

    public async Task<int> TotalRecordAsync()
    {
        return await _articleRepository.TotalRecordAsync();
    }

    public List<ArticleResponseModel> GetResponseModel(List<Article> articles)
    {
        List<ArticleResponseModel> res = new List<ArticleResponseModel>();

        foreach (var a in articles)
        {
            var categories = _articleCategoryRepository.GetCategoryList(a.Id);
            ArticleResponseModel arm = _mapper.Map<Article, ArticleResponseModel>(a);

            List<Category> category = new List<Category>();
            foreach (var c in categories)
            {
                category.Add(_categoryRepository.GetCategoryById(c.CategoryId));
            }
            arm.Categories = category;
            arm.AuthorName = _userRepository.GetUserById(a.CreateByBlogUserId).FullName;
            res.Add(arm);
        }
        
        return res; 
    }

    public async Task<List<Article>> GetPaging(int pageNumber, int pageSize)
    {
        return await _articleRepository.GetPaging(pageNumber, pageSize); 
    }
    
    public async Task<List<Article>> GetByUserNamePaging(int pageNumber, int pageSize, string username)
    {
        BlogUser user = _userRepository.GetUserByUserName(username);
        if (user == null)
        {
            return null;
        }
        
        return await _articleRepository.GetByUserIdPaging(pageNumber, pageSize, user.BlogUserId); 
    }

	public async Task<List<Article>> GetByCategoryPaging(int pageNumber, int pageSize, int categoryId) {
		return await _articleRepository.GetByCategoryPaging(pageNumber, pageSize, categoryId);
	}

	public async Task<List<Article>> GetAll()
    {
        return await _articleRepository.GetAll();
    }

    public async Task<List<Article>> GetByUserIdPaging(int pageNumber, int pageSize, int blogUserId)
    {
        return await _articleRepository.GetByUserIdPaging(pageNumber, pageSize, blogUserId);
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
        old.ModifiedDate = DateTime.Now.Date;

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
        _articleCategoryRepository.DeleteArticleCategory(id);
        _commentRepository.DeleteByArticleId(id);
        _articleRepository.Delete(article);
    }

    public async Task<List<Article>> SearchPaging(int pageNumber, int pageSize, string title)
    {
        return await _articleRepository.SearchPaging(pageNumber, pageSize, title); 
    }
}