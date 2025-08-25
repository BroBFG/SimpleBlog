using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Middleware;



var builder = WebApplication.CreateBuilder(args);

//Получаем из appsettings строку подключения
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// Сервис работы с RazorPages
builder.Services.AddRazorPages();
//Добавление логгирования
builder.Services.AddLogging();

//Добавляем аутентификацию
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });
// Добавляем авторизацию
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();
app.UseAuthentication();   // добавление middleware аутентификации 
app.UseAuthorization();   // добавление middleware авторизации 

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
//Выход из системы
app.MapGet("/logout", async (HttpContext context) =>
{
    app.Logger.LogInformation("Get /logout");
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        app.Logger.LogInformation("Пользователь {User} вышел из системы", context.User.Identity?.Name);
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    context.Response.Redirect("/");
    return StatusCodes.Status200OK;
});

app.Run();
