using Microsoft.EntityFrameworkCore;

using QuizApp.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


// Added logging for debugging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();


// Add DbContext with SQLite
builder.Services.AddDbContext<QuizDbContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:QuizDbContextConnection"]));


// Repository pattern
builder.Services.AddScoped<IQuizRepository, QuizRepository>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
        db.Database.EnsureCreated();
    }

    DBInit.Seed(app); // Ensure that DBInit.cs exists
}


// Enable static files (like CSS, JS, images)
app.UseStaticFiles();


// Enable routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Quiz}/{action=Index}/{id?}");

// Start the application
app.Run(); 