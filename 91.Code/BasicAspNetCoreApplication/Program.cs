using BasicAspNetCoreApplication;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.ReplaceConfiguration(builder.Configuration); 
builder.Services.AddApplication<AppModule>();

var app = builder.Build();

app.InitializeApplication();
app.MapRazorPages();
app.Run();
