using AutoMapper;
using EventuresApp.Data;
using EventuresApp.Models;
using EventuresApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace EventuresApp
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                     options.UseSqlServer(
                     Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<AppUser>
                (opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 4;
                opt.Password.RequiredUniqueChars = 2;
            })
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddRoles<IdentityRole>()
                  .AddDefaultTokenProviders()
                  .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, UserClaimsPrincipalFactory<AppUser, IdentityRole>>();
            services.AddSingleton<Random>();
            services.AddLogging(x =>
            {
                x.AddConsole();
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddSingleton<CreateRolesAsignAdminFirstUserMiddleware>();

            services.AddAuthentication().AddFacebook(opt =>
            {
                opt.AppId = Configuration["FBConfiguration:AppId"];
                opt.AppSecret = Configuration["FBConfiguration:AppSecret"];
            });

          services.AddHttpsRedirection(options =>
          {
              options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
              options.HttpsPort = 5001;
          });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });
            services.AddDistributedMemoryCache();
            services.AddSession(opt=> 
            {
                opt.IdleTimeout = TimeSpan.FromSeconds(20);
                opt.Cookie.HttpOnly=true;
            });

            services.AddMvc(opt =>
            {
                //  opt.Filters.Add<ValidateAntiForgeryTokenAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //  app.UseMiddleware<MyExceptionHandler>();

           app.UseExceptionHandler("/Home/MyException");
           if (env.IsDevelopment())
           {
               app.UseDeveloperExceptionPage();
               app.UseDatabaseErrorPage();
               app.UseHsts();
            }
           else
           {
               app.UseExceptionHandler("/Home/Error");
               // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
               app.UseHsts();
           }
           
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.Always,
                MinimumSameSitePolicy = SameSiteMode.Strict
            });

            app.UseAuthentication();
            app.UseSession();

             app.UseMiddleware<CreateRolesAsignAdminFirstUserMiddleware>();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                              name: "areas",
                              template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                              name: "default",
                              template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}