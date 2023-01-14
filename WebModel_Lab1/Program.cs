using WebModel_Lab1;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebModel_Lab1.Areas;
using WebModel_Lab1.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Valid_ContextConnection") ?? throw new InvalidOperationException("Connection string 'Valid_ContextConnection' not found.");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BankSystemContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User_LR2>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

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
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Accounts}/{action=Index}");
app.MapRazorPages();
app.Run();
