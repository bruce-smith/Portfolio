using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

//log with log4net
builder.Logging.AddLog4Net();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseXRay("Portfolio");
app.UseStatusCodePagesWithReExecute("/Error/e{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



//log4net.LogManager.GetCurrentLoggers()[0].Info("Start up hello");

app.Run();

