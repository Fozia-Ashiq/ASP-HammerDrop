using HammerDrop_Auction_app;
using HammerDrop_Auction_app.Entities;
using HammerDrop_Auction_app.Repositories;
using HammerDrop_Auction_app.SeedData;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/LogIn";
    });

builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddTransient<DatabaseSeeder>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConn")));




// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permission.User.ToString(), policy =>
        policy.RequireClaim("Permission", Permission.User.ToString()));

    options.AddPolicy(Permission.Admin.ToString(), policy =>
        policy.RequireClaim("Permission", Permission.Admin.ToString()));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

    if (!await dbContext.Countries.AnyAsync())
    {
        Console.WriteLine("Database is empty, no data to read.");
    }
    else
    {
        var countries = await seeder.GetAllCountriesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
   
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LogIn}/{id?}");

app.Run();
