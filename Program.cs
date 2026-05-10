using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Configurations;
using VitrineSemiJoias.Data;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program).Assembly);
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? throw new InvalidOperationException("A variável 'DB_CONNECTION' não foi encontrada no arquivo .env"); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
