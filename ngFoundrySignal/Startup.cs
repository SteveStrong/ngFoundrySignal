using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ngFoundrySignal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Swagger;

namespace ngFoundrySignal
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });


            services.AddCors(options => {
                options.AddPolicy("allowAny", x => {
                    x.AllowAnyHeader();
                    x.AllowAnyMethod();
                    x.AllowAnyOrigin();
                });
            });


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "ngFoundrySignal",
                    Version = "v1",
                    Description = "Full API to support ngFoundry collaberation",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Steve Strong",
                        Email = string.Empty,
                        Url = "https://twitter.com/SteveStrong"
                    },
                    // License = new License
                    // {
                    //     Name = "Use under LICX",
                    //     Url = "https://example.com/license"
                    // }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ngFoundrySignal API V1");
                //c.RoutePrefix =  string.Empty;
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseCors("allowAny");
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
                routes.MapHub<ShapeHub>("/shapehub");
            });


            app.UseMvc();

        }
    }
}
