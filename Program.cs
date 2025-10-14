using Microsoft.EntityFrameworkCore;

using QuizApp.DAL;



var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();



// ✅ Legg til logging

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Logging.AddDebug();



// ✅ Legg til DbContext

builder.Services.AddDbContext<QuizDbContext>(options =>

    options.UseSqlite(builder.Configuration["ConnectionStrings:QuizDbContextConnection"]));



// ✅ Repository pattern

builder.Services.AddScoped<IQuizRepository, QuizRepository>();



var app = builder.Build();



if (app.Environment.IsDevelopment())

{

    app.UseDeveloperExceptionPage();



    using (var scope = app.Services.CreateScope())

    {

        var db = scope.ServiceProvider.GetRequiredService<QuizDbContext>();

        db.Database.EnsureCreated();

    }



    DBInit.Seed(app); // ✅ sørg for at DBInit.cs eksisterer

}



app.UseStaticFiles();



app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Quiz}/{action=Index}/{id?}");



app.Run(); 