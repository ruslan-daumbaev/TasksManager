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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            logger.LogDebug("Started services configuration");
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var conString = Configuration["ConnectionStrings:TMConnectionString"];
            services.AddDbContext<TasksManagerDbContext>(options => options.UseSqlServer(conString));
            services.AddTransient<ITasksService, TasksService>();
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // In production, the Angular files will be served from this directory
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info {Title = "TasksManager API", Version = "v1"}); });
          

            logger.LogDebug("Finished services configuration");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAntiforgery antiforgery)
        {

            app.UseCors(builder =>
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            );

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TasksManager API V1"); });

            app.UseHttpsRedirection();
            app.UseMvc();

            app.Use(next => context =>
            {
                string path = context.Request.Path.Value;
                if (string.Equals(path, "/", StringComparison.OrdinalIgnoreCase))
                {
                    // The request token can be sent as a JavaScript-readable cookie, 
                    // and Angular uses it by default.
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                        new CookieOptions() {HttpOnly = false});
                }

                return next(context);
            });


            logger.LogDebug("Finished app configuration");
        }
    }
}