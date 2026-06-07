using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);


builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada no appsettings.json");
    
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserModel, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddErrorDescriber<IdentityMensagensPortugues>();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<UserModel>, UserClaimsPrincipalFactory>();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim("Profile", ProfileEnum.Admin.ToString()));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
builder.Services.Configure<MostruarioOptions>(builder.Configuration.GetSection("Mostruario"));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddSmartServices();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
public partial class Program { }