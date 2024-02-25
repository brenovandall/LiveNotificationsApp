using LiveNotificationsMVC.Data;
using LiveNotificationsMVC.Hubs;
using LiveNotificationsMVC.MiddlewareExtensions;
using LiveNotificationsMVC.Repository;
using LiveNotificationsMVC.SubscribeTableDependencies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

var connectionString = builder.Configuration.GetConnectionString("stringApp");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString), ServiceLifetime.Singleton);

builder.Services.AddScoped<IUserInterface, UserInterface>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<NotificationHub>();
builder.Services.AddSingleton<SubscribeTableDependency>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapHub<NotificationHub>("/notifications");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=SignIn}");

app.UseSqlTableDependency<SubscribeTableDependency>(connectionString);

app.Run();
