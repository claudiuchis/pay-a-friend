using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

using Pay.WebApp.Configs;

namespace Pay.WebApp
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
            services.AddControllersWithViews();
            services.AddOptions();

            services
                .Configure<IdentityProviderConfiguration>(Configuration.GetSection("IdentityConfig"))
                .Configure<ApiConfiguration>(Configuration.GetSection("ApiConfig"))
                .Configure<LocalUrls>(Configuration.GetSection("LocalUrls"))
                .AddCustomAuthentication()
                .AddCustomServices()
                .AddHttpContextAccessor()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}")
                .RequireAuthorization()
                ;
            });
        }
    }

    public static class StartupExtension
    {
        public static IServiceCollection AddCustomAuthentication(
            this IServiceCollection services)
        {

            var sp = services.BuildServiceProvider();
            var identityConfig = sp.GetService<IOptions<IdentityProviderConfiguration>>();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", options => {
                options.ExpireTimeSpan = new TimeSpan(0, 30, 0); // cookie to expire after 30 minutes
            })
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = identityConfig.Value.Authority;

                options.ClientId = identityConfig.Value.ClientId;
                options.ClientSecret = identityConfig.Value.ClientSecret;
                options.ResponseType = "code";
                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";

                // add api related scopes
                options.Scope.Add("pay.verification");

                options.SaveTokens = true;
            });
            return services;
        }

        public static IServiceCollection AddCustomServices(
            this IServiceCollection services
        )
        {
            services
                .AddSingleton<VerificationService>();

            return services;
        }
    }
}
