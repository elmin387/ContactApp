using ContactApp.Application;
using ContactApp.Infrastructure;
using Serilog;
using System.IO;

string logPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
if (!Directory.Exists(logPath))
{
    Directory.CreateDirectory(logPath);
}

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddControllersWithViews();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

try
{
    Log.Information("Starting the app...");

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
        pattern: "{controller=Contact}/{action=Create}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application stopped");
}
finally
{
    Log.CloseAndFlush();
}