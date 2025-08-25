using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleBlog.Db;
using SimpleBlog.Models;

namespace SimpleBlog.Pages
{
    [IgnoreAntiforgeryToken]
    public class RegModel : PageModel
    {
        
        private ILogger<RegModel> logger;
        private ApplicationContext db;


        public RegModel(ILogger<RegModel> logger, ApplicationContext db)
        {
            this.logger = logger;
            this.db = db;
        }
        public void OnGet()
        {
        }
        // Добавление нового пользователя
        [BindProperty]
        public Input_Model Input { get; set; } = new();
        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation("RegModel.OnPostAsync called");
            if (Input.Password != Input.Confirm_password)
            {
                TempData["Message"] = "Не верное подтверждение пароля";
                return Page();
            }
            var user = new Db.Models.User
            {
                Nickname = Input.Nickname,
                Password = Input.Password,
                Role = db.Roles.FirstOrDefault(r => r.Name == "user") 
            };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return RedirectToPage("/Auth");
        }
    }
}
