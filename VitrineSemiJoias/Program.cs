using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Services;
using VitrineSemiJoias.Services.Interfaces;

// 1. Carregar variáveis de ambiente ANTES de inicializar o WebApplication Builder
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Adiciona as variáveis de ambiente carregadas pelo .env no Configuration do .NET
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// 3. Gerenciamento de Cache e Sessão
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// 4. Banco de Dados e Identity
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");
    
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<UserModel, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Fábrica de Claims Customizada
builder.Services.AddScoped<IUserClaimsPrincipalFactory<UserModel>, UserClaimsPrincipalFactory>();

// 5. Autenticação, Cookies e Autorização
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser()
              .RequireClaim("Profile", ProfileEnum.Admin.ToString()));
});

// 6. Strongly Typed Options (Opções Fortemente Tipadas)
builder.Services.Configure<GeminiOptions>(builder.Configuration.GetSection("Gemini"));
builder.Services.Configure<MostruarioOptions>(builder.Configuration.GetSection("Mostruario"));
builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpSettings"));

// 7. Serviços de Domínio e Clientes HTTP
builder.Services.AddSmartServices(); 
// Certifique-se de que AddSmartServices() NÃO registra o IGeminiService para evitar duplicidade conflituosa com a linha abaixo:
builder.Services.AddHttpClient<IGeminiService, GeminiService>();

var app = builder.Build();

// 8. Pipeline de Middlewares (HTTP Request Pipeline)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Sessão precisa rodar ANTES da Autenticação/Autorização caso os dados do usuário dependam dela
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public partial class Program { }