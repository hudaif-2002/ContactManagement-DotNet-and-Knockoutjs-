//using Microsoft.EntityFrameworkCore;
//using ContactManagement.Models;
//using ContactManagement.Data;


//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();


//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));


//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});


//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseSession();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();








using Microsoft.EntityFrameworkCore;
using ContactManagement.Models;
using ContactManagement.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Add DbContext with connection string
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));

// Add Distributed Memory Cache for Session Management
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add Swagger services
builder.Services.AddSwaggerGen(options =>
{
    // Optional: You can provide metadata for your API in Swagger
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Contact Management API",
        Version = "v1",
        Description = "An API for managing contacts"
    });
    options.EnableAnnotations();
});

var app = builder.Build();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/auth/login");
        return;
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development mode
    app.UseSwagger();  // Enable Swagger
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Management API V1");
        //options.RoutePrefix = string.Empty; // To have Swagger UI accessible at the root of the app (optional)
        options.RoutePrefix = "swagger";
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();  // Enable session management
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();








//using Microsoft.EntityFrameworkCore;
//using ContactManagement.Models;
//using ContactManagement.Data;
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddControllersWithViews();  // Enables MVC (views, controllers)

//// Add DbContext with connection string
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerDb")));

//// Add Distributed Memory Cache for Session Management
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set timeout for sessions
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//var app = builder.Build();

//// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{

//     app.UseSwagger();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact Management API V1");
//        options.RoutePrefix = string.Empty;
//    });
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//// Serve static files (e.g., index.html, CSS, JS)
//app.UseStaticFiles();  // Enables static file serving

//// Enable default files like index.html
//app.UseDefaultFiles();  // Default to index.html if exists in wwwroot

//app.UseHttpsRedirection();
//app.UseRouting();
//app.UseSession();  // Enable session management
//app.UseAuthorization();

//// Default route for MVC (views)
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Auth}/{action=Login}/{id?}");

//// Run the application
//app.Run();
