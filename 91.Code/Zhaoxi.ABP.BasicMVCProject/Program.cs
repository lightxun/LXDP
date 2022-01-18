using Zhaoxi.ABP.BasicMVCProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.ReplaceConfiguration(builder.Configuration);
builder.Services.AddApplication<AppModule>();

var app = builder.Build();

app.InitializeApplication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
