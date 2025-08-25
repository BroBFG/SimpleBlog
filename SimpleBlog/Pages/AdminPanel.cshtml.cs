using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Db.Models;

namespace SimpleBlog.Pages
{
    [IgnoreAntiforgeryToken]
    [Authorize (Roles = "admin")]
    public class AdminPanelModel : PageModel
    {
        ApplicationContext db;

        ILogger<AdminPanelModel> logger;
        public List<User> Users { get; set; } = new();

        public AdminPanelModel(ApplicationContext db, ILogger<AdminPanelModel> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public void OnGet()
        {
            Users = db.Users
                .Include(u => u.Role)
                .AsNoTracking()
                .ToList();
        }
        // Добавление нового поста
        [BindProperty]
        public Post Post { get; set; } = new();
        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation("AdminPanelModel.OnPostAsync called");

            Post.Date = DateOnly.FromDateTime(DateTime.Today);
            await db.AddAsync(Post); 
            await db.SaveChangesAsync();

            return  RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            logger.LogInformation("AdminPanelModel.OnPostDeleteAsync called");
            var user = await db.Users.FindAsync(id);

            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
