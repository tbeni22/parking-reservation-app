using DataAccess;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using BusinessLogic.Interfaces;
using BusinessLogic.Interfaces.Implementations;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var connectionString = builder.Configuration.GetConnectionString("ParkingContext") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ParkingContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


//Todo: add options to service
//builder.Services.AddIdentity<User, IdentityRole<int>>()
//.AddEntityFrameworkStores<ParkingContext>();

builder.Services.AddDefaultIdentity<User>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
    .AddRoles<IdentityRole<int>>()
    .AddRoleManager<RoleManager<IdentityRole<int>>>()
    .AddEntityFrameworkStores<ParkingContext>();


builder.Services.ConfigureApplicationCookie(options =>
{
    // TODO: Pathing
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;

});

builder.Services.AddScoped(typeof(IStatistics), typeof(StatisticsService));
builder.Services.AddScoped(typeof(IUserManagement), typeof(UserService));
builder.Services.AddScoped(typeof(IParkingPlace), typeof(ParkingService));
builder.Services.AddScoped(typeof(IReservation), typeof(ReservationManager));
builder.Services.AddSingleton(typeof(EmailService));


var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetService<ParkingContext>();

    db.Database.Migrate();

    await SeedManager.Seed(services);

    var emailService = services.GetService<EmailService>();

}

app.Run();

