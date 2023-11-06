using web_blog.Entities;

namespace web_blog.Context;

public static class TasksInitializer
{
    public static WebApplication Seed(this WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            using var context = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
            try
            {
                context.Database.EnsureCreated();

                var articleCateogory = context.ArticleCategories.FirstOrDefault();
                context.ArticleCategories.AddRange(
                    new () { ArticleId = -5, CategoryId = -1},
                    new () { ArticleId = -1, CategoryId = -4},
                    new () { ArticleId = -1, CategoryId = -3},
                    new () { ArticleId = -2, CategoryId = -2},
                    new () { ArticleId = -2, CategoryId = -1},
                    new () { ArticleId = -3, CategoryId = -3},
                    new () { ArticleId = -4, CategoryId = -4}
                );
                context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return app;
        }
    }
}