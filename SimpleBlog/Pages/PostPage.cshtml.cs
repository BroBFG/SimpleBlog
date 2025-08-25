using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Db.Models;
using System.Xml.Schema;

namespace SimpleBlog.Pages
{
    [IgnoreAntiforgeryToken]
    public class PostPageModel : PageModel
    {
        private readonly ILogger<PostPageModel> logger;
        private ApplicationContext db;
        public Post? Post { get; set; } = new();
        public PostPageModel(ILogger<PostPageModel> logger, ApplicationContext db)
        {
            this.logger = logger;
            this.db = db;
        }

        public void OnGet(int id)
        {
            if(id <= 0)
            {
                logger.LogError("Invalid post ID: {Id}", id);
                RedirectToPage("/Index");
            }
            else
            {

                Post = db.Posts
                    .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                    .FirstOrDefault(p => p.Id == id);
                if (Post == null)
                {
                    logger.LogWarning("Post with ID {Id} not found", id);
                    RedirectToPage("/Index");
                }
            }
        }
        [BindProperty]
        public Comment NewComment { get; set; }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            logger.LogInformation("OnPostCommentAsync called for post ID: {PostId}", id);

            //Comment NewComment = new();
            NewComment.User_Id = db.Users.AsNoTracking().FirstOrDefault(db => db.Nickname == User.Identity.Name).Id;
            NewComment.Post_Id = id;
            NewComment.Id = 0;

            await db.Comments.AddAsync(NewComment);
            await db.SaveChangesAsync();
            return RedirectToPage("/postpage", new { id });
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            logger.LogInformation("PostPageModel PostDelete id:{0}", id);
            var com = await db.Comments.FindAsync(id);
            int idp = com.Post_Id;
            if (com != null)
            {
                db.Comments.Remove(com);
                await db.SaveChangesAsync();
            }
            return RedirectToPage("/postpage", new { id = idp });
        }
    }
}
