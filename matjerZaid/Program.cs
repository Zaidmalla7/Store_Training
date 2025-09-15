using ECApp.Data;
using ECApp.Model;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using StoreOn.Models;
using System.Text.Json;
using Zaid.Services;


namespace ECApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          

            builder.Services.AddControllersWithViews();
            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionstring));

            builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero; 
            });

            // Identity + Authentication
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });




            var app = builder.Build();
            app.MapPost("/device-check", async (HttpContext ctx) =>
            {
                try
                {
                    // ???? JSON
                    using var sr = new StreamReader(ctx.Request.Body);
                    var body = await sr.ReadToEndAsync();
                    if (string.IsNullOrWhiteSpace(body))
                    {
                        ctx.Response.StatusCode = 400;
                        await ctx.Response.WriteAsync("{\"allowed\":false}");
                        return;
                    }

                    var doc = JsonDocument.Parse(body);
                    var root = doc.RootElement;

                    int width = root.TryGetProperty("width", out var w) ? w.GetInt32() : 0;
                    int height = root.TryGetProperty("height", out var h) ? h.GetInt32() : 0;
                    int touch = root.TryGetProperty("touch", out var t) ? t.GetInt32() : 0;

                    // ????? ????: ?? ????? ??? ?? 900 ?? ??? touch points => ????? Mobile
                    bool isMobile = (width > 0 && width < 900) || (touch > 0);

                    if (isMobile)
                    {
                        // ?? ???? ???? ??????? (HttpOnly) — ?? ???? ???????? ?? JS
                        ctx.Response.Cookies.Append("VerifiedDevice", "Mobile", new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = ctx.Request.IsHttps,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(5)
                        });

                        ctx.Response.ContentType = "application/json";
                        await ctx.Response.WriteAsync("{\"allowed\":false}");
                        return;
                    }
                    else
                    {
                        // Desktop => ?? ???? Desktop (HttpOnly? ?? ???? ??????? ?? JS)
                        ctx.Response.Cookies.Append("VerifiedDevice", "Desktop", new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = ctx.Request.IsHttps,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(10)
                        });

                        ctx.Response.ContentType = "application/json";
                        await ctx.Response.WriteAsync("{\"allowed\":true}");
                        return;
                    }
                }
                catch (Exception)
                {
                    ctx.Response.StatusCode = 500;
                    ctx.Response.ContentType = "application/json";
                    await ctx.Response.WriteAsync("{\"allowed\":false}");
                }
            });

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<BlockMobileForAdminMiddleware>();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.MapRazorPages();

            app.Run();
        }
    }
}
