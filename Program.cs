using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Data;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

builder.Services.AddControllersWithViews();
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION")
    ?? throw new InvalidOperationException("A variável 'DB_CONNECTION' não foi encontrada no arquivo .env"); 

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

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
