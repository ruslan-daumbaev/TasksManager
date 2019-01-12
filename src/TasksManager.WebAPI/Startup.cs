using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using TasksManager.Data.DataContext;
using TasksManager.Services.Contracts;
using TasksManager.Services.Implementation;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var conString = Configuration["ConnectionStrings:TMConnectionString"];
            services.AddDbContext<TasksManagerDbContext>(options => options.UseSqlServer(conString));
            services.AddTransient<ITasksService, TasksService>();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Add Swagger service
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "TasksManager API", Version = "v1"}); });
            services.AddSignalR();

            logger.LogDebug("Finished services configuration");
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
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
            app.UseMvc();

            app.Use(next => context =>
            {
                var path = context.Request.Path.Value;
                if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase))
                {
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions {HttpOnly = false});
                }

                return next(context);
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<TasksHub>("/tasks-ws");
            });


            logger.LogDebug("Finished app configuration");
        }
    }
}