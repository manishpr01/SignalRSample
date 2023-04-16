using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Data;
using SignalRSample.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//builder.Services.AddSignalR();

var connectionAzureSignalR = "Endpoint=https://dotnetsignalr.service.signalr.net;AccessKey=abo4HMnbak36Fg+z5Ev+dw4RmQGPhFlyPVUoxzUIDtU=;Version=1.0;";

builder.Services.AddSignalR().AddAzureSignalR(connectionAzureSignalR);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<UserHub>("/hubs/userCount");
app.MapHub<DeathlyHallowsHub>("hubs/deathyhallows");
app.MapHub<HouseGroupHub>("/hubs/houseGroup");
app.MapHub<NotificationHub>("/hubs/notification");
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<OrderHub>("/hubs/order");

app.Run();
