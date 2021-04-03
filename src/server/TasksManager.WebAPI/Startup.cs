using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using TasksManager.Data.DataContext;
using TasksManager.Services;
using TasksManager.Services.Interfaces;
using TasksManager.WebAPI.Hubs;
using TasksManager.WebAPI.Infrastructure;

namespace TasksManager.WebAPI
{
    public class Startup
    {
        private readonly ILogger logger;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            this.logger = logger;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogDebug("Started services configuration");
            services.AddCors();
            services.AddSignalR();
            services.AddControllers();

            var conString = Configuration["ConnectionStrings:TasksDb"];
            services.AddDbContext<TasksDbContext>(options => options.UseSqlServer(conString));
            services.AddTransient<ITasksService, TasksService>();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Add Swagger service
            services.AddSwaggerGen();
            logger.LogDebug("Finished services configuration");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IAntiforgery antiforgery)
        {
            //for test allow all CORS operations
            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials());

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
                app.UseHsts();
            }

            app.UseSwagger();

            // Enable Swagger endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksManager API V1"); });

            app.UseHttpsRedirection();

            app.Use(next => context =>
            {
                var path = context.Request.Path.Value;
                if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions { HttpOnly = false });
                }

                return next(context);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<TasksHub>("/tasks-ws");
            });


            logger.LogDebug("Finished app configuration");
        }
    }
}