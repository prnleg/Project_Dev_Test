using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using Project_Dev_Test.Web.Service;
using Project_Dev_Test.Web.Repository;

namespace Project_Dev_Test.Web
{
    public class Startup
    {
        public Startup(IConfiguration config) => this.Configuration = config;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = int.MaxValue;
            });

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
                o.BufferBodyLengthLimit = int.MaxValue;
                o.BufferBody = true;
                o.ValueCountLimit = int.MaxValue;
            });

            services.AddScoped<DataRepository>();
            services.AddScoped<AlgorithmService>();

            services.AddControllersWithViews();
            //.AddNewtonsoftJson();

            services.AddRazorPages();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseRouting();

            DataRepository.InitializeDatabase();
            Helpers.MatrixModel.Initialize();

            app.UseStaticFiles();
            app.UseCookiePolicy();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1.2.1");
            });
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }

    }
}
