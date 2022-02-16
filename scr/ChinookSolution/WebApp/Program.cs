
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;

#region Additional Namespaces
using ChinookSystem;
#endregion
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//supplied database connection due to the fact that we create this
//  web app to use Individual Accounts
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//retrieve the connect string for Chinook from the appsetting.json file
var connectionStringChinook = builder.Configuration.GetConnectionString("ChinookDB");


//given for the dbconnection to the DefaultConnection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//code the dbconnection to the application DB context for Chinook
//the implementation of the connect AND registration of the ChinookSystem services
//  will be done in the ChinookSystem class library
//so accomplish this task, we will use an "extension method"
//the extension method will extend the IServiceCollection class
//the extension method requires a parameter options.UseSqlServer(xxxx)
builder.Services.ChinookSystemBackendDependencies(options =>
    options.UseSqlServer(connectionStringChinook));

//given for application use
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.MapRazorPages();

app.Run();
