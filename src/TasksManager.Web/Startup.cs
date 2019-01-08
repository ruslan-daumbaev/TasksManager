using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TasksManager.Data.DataContext;
using TasksManager.Services.Contracts;
using TasksManager.Services.Implementation;
using TasksManager.Web.Infrastructure;

namespace TasksManager.Web
{
    public class Startup
    {
        private readonly ILogger logger;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogDebug("Started services configuration");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var conString = configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<TasksManagerDbContext>(options => options.UseSqlServer(conString));
            services.AddTransient<ITasksService, TasksService>();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(cfg => { cfg.RootPath = "ClientApp/dist"; });

            logger.LogDebug("Finished services configuration");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {
            logger.LogDebug("Started app configuration");

            if (env.IsDevelopment())
            {
                logger.LogDebug("Development environment is used");

                app.UseDeveloperExceptionPage();
                app.UseMiddleware<ExceptionMiddleware>();
            }
            else
            {
                logger.LogDebug("Production environment is used");

                app.UseMiddleware<ExceptionMiddleware>();
                app.UseExceptionHandler();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.Use(next => context =>
            {
                string path = context.Request.Path.Value;
                if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase))
                {
                    // The request token can be sent as a JavaScript-readable cookie, 
                    // and Angular uses it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() { HttpOnly = false });
                }

                return next(context);
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            logger.LogDebug("Finished app configuration");
        }
    }
}