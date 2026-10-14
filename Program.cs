
using CarInsurance.Data;
using Microsoft.EntityFrameworkCore;

// Create the application builder.
var builder = WebApplication.CreateBuilder(args);

// Register MVC services.
builder.Services.AddControllersWithViews();

// Configure the SQL Server database connection.
builder.Services.AddDbContext<InsuranceContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("InsuranceContext")));

// Build the application.
var app = builder.Build();

// Configure error handling.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS.
app.UseHttpsRedirection();

// Serve static files such as CSS and images.
app.UseStaticFiles();

// Enable routing.
app.UseRouting();

// Enable authorization.
app.UseAuthorization();

// Configure the default MVC route.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Start the application.
app.Run();
