using DataAccess;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;

using BuisnessLogic.Interfaces;
using BuisnessLogic.Interfaces.Implementations;
using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using BusinessLogic.DTOs;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var connectionString = builder.Configuration.GetConnectionString("ParkingContext") ?? throw new InvalidOperationException("Connection string not found.");
builder.Services.AddDbContext<ParkingContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDefaultIdentity<User>
    (options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;
        options.Stores.ProtectPersonalData = true;
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
    })
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

builder.Services.AddScoped(typeof(WeatherForecastService));
builder.Services.AddScoped(typeof(IStatistics), typeof(StatisticsService));

var app = builder.Build();

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

app.Run();

