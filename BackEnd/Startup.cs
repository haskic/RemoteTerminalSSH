using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebSocketManager;
using BackEnd.SocketControllers;
using Microsoft.EntityFrameworkCore;
using BackEnd.Data;
using BackEnd.Controllers;

namespace BackEnd
{
    public class Startup
    {
        public string connectionString = "Data Source=DESKTOP-G92CTF5;Initial Catalog=ZhakarSSH;Integrated Security=True";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddCors(options => options.AddPolicy("CorsPolicy",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .SetIsOriginAllowed((host) => true)
                   .AllowCredentials();
        }));
            services.AddWebSocketManager();

            services.AddSignalR();
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILogger<Program> logger)
        {
            logger.LogWarning("I'm READY");
            app.UseCors("CorsPolicy");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseWebSockets();
            //app.MapWebSocketManager("/socket", serviceProvider.GetService<SocketTerminal>());


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TerminalHub>("/terminal");

                endpoints.MapHub<SearchHub>("/search");

                //endpoints.MapControllers();
            });
        }
    }
}
