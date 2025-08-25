using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Db.Models;

namespace SimpleBlog.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private ApplicationContext db;
        public List<Post> Posts { get; set; } = new();

        public IndexModel(ILogger<IndexModel> logger, ApplicationContext db)
        {
            this.logger = logger;
            this.db = db;
        }
                                                                                                                                                                
        public void OnGet()
        {
            Posts = db.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .OrderByDescending(p => p.Date)
                .ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            logger.LogInformation("IndexModel.OnPostDeleteAsync called for post ID: {PostId}", id);
            var post = await db.Posts.FindAsync(id);
            if (post != null)
            {
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
                logger.LogInformation("Post with ID {PostId} deleted successfully", id);
            }
            else
            {
                logger.LogWarning("Post with ID {PostId} not found for deletion", id);
            }
            return RedirectToPage();
        }
    }
}
