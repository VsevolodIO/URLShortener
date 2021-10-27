using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using URLShortener.Areas.Identity.Authorization;
using URLShortener.Areas.Identity.Services;
using URLShortener.Data;
using URLShortener.Services;
using URLShortener.Services.DependencyInjection;
using URLShortener.Areas.Identity.Data;
using URLShortener.Areas.Identity.Services.Configurations;


namespace URLShortener
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
            services.AddMvc(/*options => options.Filters.Add(new AuthorizeFilter())*/);
            services.AddDbContextPool<UrlShortenerDbContext>((IServiceProvider sp, DbContextOptionsBuilder options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                var env = sp.GetRequiredService<IWebHostEnvironment>();
                options.EnableSensitiveDataLogging(/*env.IsDevelopment()*/);
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                options.LogTo(Console.WriteLine, LogLevel.Information);
            });

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<UrlShortenerDbContext>();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUserUrlData, UserUrlData>();
            services.AddScoped<IAnonUrlData, AnonUrlData>();
            services.AddScoped<IAdminData, AdminData>();

            services.AddAppConfiguration(Configuration)
                    .AddAnonUrlRules();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddSingleton<IRecaptchaService, GoogleRecaptchaService>();

            services.AddSingleton<IUrlGenerator, ShortUrlGenerator>();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddScoped<IAuthorizationHandler, UrlIsOwnerAuthorizationHandler>();
            services.AddSingleton<IAuthorizationHandler, UrlAdministratorsAuthorizationHandler>();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider sp, IOptions<IdentityConfiguration> opt)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            AppContext.SetSwitch(
                "Microsoft.AspNetCore.Authorization.SuppressUseHttpContextAsAuthorizationResource",
                isEnabled: true);
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllerRoute(
                //    name: "urls",
                //    pattern: "~/{action=Index}URL/{id?}",
                //    defaults: new { controller = "UserUrls" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=AnonUrl}/{action=Index}/{id?}");
                //endpoints.MapControllerRoute(
                //    name: "default2",
                //    pattern: "{controller=Urls}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default1",
                    pattern: "{ShortUrl?}");
                endpoints.MapRazorPages();
            });

            IdentityOnStartupConfigurator.CreateRoles(sp, opt).Wait();
            IdentityOnStartupConfigurator.CreateAdmin(sp, opt).Wait();
        }
    }
}
