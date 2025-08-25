using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Db;
using SimpleBlog.Middleware;



var builder = WebApplication.CreateBuilder(args);

//�������� �� appsettings ������ �����������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");

// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// ������ ������ � RazorPages
builder.Services.AddRazorPages();
//���������� ������������
builder.Services.AddLogging();

//��������� ��������������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth";
        options.AccessDeniedPath = "/";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });
// ��������� �����������
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseMiddleware<ErrorMiddleware>();
app.UseAuthentication();   // ���������� middleware �������������� 
app.UseAuthorization();   // ���������� middleware ����������� 

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
//����� �� �������
app.MapGet("/logout", async (HttpContext context) =>
{
    app.Logger.LogInformation("Get /logout");
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        app.Logger.LogInformation("������������ {User} ����� �� �������", context.User.Identity?.Name);
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
    context.Response.Redirect("/");
    return StatusCodes.Status200OK;
});

app.Run();
