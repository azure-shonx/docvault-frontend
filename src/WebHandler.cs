using Microsoft.AspNetCore.Mvc;

public class WebHandler
{
    public WebHandler(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

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

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{FileName?}");

     /*   app.MapControllerRoute(
           name: "Download",
           pattern: "Document/Download/{FileName}",
           new { controller = "Document", action = "Download" });

        app.MapControllerRoute(
           name: "Download",
           pattern: "Document/Edit/{id?}",
           new { controller = "Document", action = "Edit" });

    */

        app.Run();

    }
}
