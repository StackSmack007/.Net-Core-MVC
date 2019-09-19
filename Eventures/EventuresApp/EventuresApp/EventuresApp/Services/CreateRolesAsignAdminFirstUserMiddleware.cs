namespace EventuresApp.Services
{
    using EventuresApp.Data;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;
    using EventuresApp.Models;
    public class CreateRolesAsignAdminFirstUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CreateRolesAsignAdminFirstUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext, ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            if (!db.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //httpContext.Response.Headers.Add("testing", "mesting");
            await _next(httpContext);
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var user = db.Users.First(x => x.UserName == httpContext.User.Identity.Name);
                bool isUserAdmin = db.Users.Count() == 1;
                if (!httpContext.User.IsInRole("User"))
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                if (!httpContext.User.IsInRole("Admin")&& isUserAdmin)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}