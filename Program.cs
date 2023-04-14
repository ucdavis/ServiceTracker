using Microsoft.EntityFrameworkCore;
using ServiceTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using GSS.Authentication.CAS.AspNetCore;
using ServiceTracker.Services;
using Microsoft.AspNetCore.Server.IISIntegration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages()
            .AddRazorRuntimeCompilation();        

        builder.Services.AddDbContext<ServiceTrackerContext>(o =>
                    {
                        o.UseSqlServer(builder.Configuration.GetConnectionString("ServiceTrackerContext"));
                        o.UseLoggerFactory(ServiceTrackerContext.GetLoggerFactory());
                    });
                  
        
        builder.Services.AddScoped<IdentityService, IdentityService>();
        builder.Services.AddAuthentication("Cookies")
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            })
            .AddCAS(o =>
            {
                o.SignInScheme = "Cookies";
                o.CasServerUrlBase = builder.Configuration["CasBaseUrl"];
                o.Events.OnCreatingTicket = async context =>
                {
                    if (context.Identity == null)
                    {
                        return;
                    }
                    var ident = (ClaimsIdentity) context.Principal.Identity;
                    var assertion = context.Assertion;
                    var kerb = assertion.PrincipalName;
                    if (string.IsNullOrWhiteSpace(kerb)) return;
                    ident.AddClaim(new Claim(ClaimTypes.NameIdentifier, assertion.PrincipalName));
                    var db = context.HttpContext.RequestServices.GetRequiredService<ServiceTrackerContext>();
                    var user = await db.Employees.Where(e => e.Current && e.KerberosId == kerb).FirstOrDefaultAsync();
                    if(user != null)
                    {
                        ident.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                        ident.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));

                        if(user.AdminStaff && user.Chair)
                        {
                            ident.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                            ident.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
                        }
                    }
                    context.Principal.AddIdentity(ident);

                    return;
                };
            }); 

        // builder.Services.AddAuthorization(options =>
        // {
        //     // Globally Require Authenticated Users
        //     options.FallbackPolicy = new AuthorizationPolicyBuilder()
        //         .RequireAuthenticatedUser()
        //         .Build();
        // });
        

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
        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}