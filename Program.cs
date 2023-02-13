using MaMontreal;
using MaMontreal.Data;
using MaMontreal.Models;
using MaMontreal.Repositories;
using MaMontreal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

// string connectionString = builder.Configuration.GetConnectionString("AppConfig");
// builder.Configuration.AddAzureAppConfiguration("Endpoint=https://mamontrealappconfig.azconfig.io;Id=/Fpl-l0-s0:ND3ee5JETSPr3CHzbp7I;Secret=cQ3t9J90DD0Dp+AeEsIOrV5h15XJ91IFpFHnl+5ukck=");

builder.Services.AddDbContext<MamDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MamDbContext") ?? throw new InvalidOperationException("Connection string 'MamDbContext' not found.")));

builder.Services
.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<MamDbContext>();

//Add email services
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

// Add services to the container.
builder.Services.AddControllersWithViews();

//Azure Blob
builder.Services.AddTransient<AzureStorageService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// app.MapControllerRoute(
// name: "default",
// pattern: "{controller=Home}/{action=Index}/{id?}"
// );

// app.MapControllerRoute(name: "manage",
//                 pattern: "/manage",
//                 defaults: new { controller = "Manage", action = "Index" });

// app.MapControllerRoute(
//    name: "manage_requests",
//    pattern: "manage/requests/{action=Index}/{id?}",
//    defaults: new { controller = "ManageRequests" }
//    );

// app.MapControllerRoute(
// name: "manage_users",
// pattern: "manage/users/{action=Index}/{id?}",
// defaults: new { controller = "ManageUsers" }
// );


app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MamDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    db.Database.EnsureCreated();
    SeedData.SeedRoles(db, roleManager);
    SeedData.AddRole("aftab@hotmail.com", "admin", userManager);
    SeedData.AddRole("aftab@hotmail.com", "gsr", userManager);
    SeedData.AddRole("aftab@hotmail.com", "member", userManager);
    // SeedData.RemoveRole("aftab@hotmail.com", "admin", userManager);


    SeedData.AddRole("julieta.ja@gmail.com", "admin", userManager);
    SeedData.AddRole("julieta.ja@gmail.com", "gsr", userManager);
    SeedData.AddRole("julieta.ja@gmail.com", "member", userManager);
    // SeedData.RemoveRole("aftab@hotmail.com", "admin", userManager);
}

app.Run();