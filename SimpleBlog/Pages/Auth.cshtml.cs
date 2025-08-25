using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Models;
using System;
using System.Security.Claims;

namespace SimpleBlog.Pages
{
    [IgnoreAntiforgeryToken]
    public class AuthModel : PageModel
    {
        private ApplicationContext db;
        private ILogger<AuthModel> logger;

        public AuthModel(ApplicationContext db, ILogger<AuthModel> logger)
        {
            this.db = db;
            this.logger = logger;
        }
        public void OnGet()
        {
        }
        [BindProperty]
        public Input_Model Input { set; get; } = new();
        public async Task<IActionResult> OnPostAsync()
        {
            logger.LogInformation("AuthModel.OnPostAsync called with nickname: {0}", Input.Nickname);
            var context = HttpContext;
            var user =  db.Users.Include(u => u.Role).AsNoTracking().FirstOrDefault(u => u.Nickname == Input.Nickname && u.Password == Input.Password);
            if (user == null)
            {
                TempData["Message"] = "Неверное имя пользователя или пароль.";
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Nickname),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal);
            logger.LogInformation("User {0} authenticated successfully", user.Nickname);
            if(user.Role.Name == "admin")
            {
                return RedirectToPage("/AdminPanel");
            }
            else
            {
                return RedirectToPage("/Index");
            }
        }
    }
}
