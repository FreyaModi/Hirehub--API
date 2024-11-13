
using MailKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace lifesync
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
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            var assembly = typeof(HirehubController).Assembly;
            services.AddControllers().PartManager.ApplicationParts.Add(new AssemblyPart(assembly));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Hirehub  -Placement Management system",
                    Version = "v1",
                    Description = "Contains all the API endpoints deveoped for building hirehub.",
                    Contact = new OpenApiContact
                    {
                        Name = "Freya Modi",
                      
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            services.AddControllers();
            //     services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookStoreDB")));
            services.AddControllers().AddNewtonsoftJson();
            //services.AddTransient<IBookRepository, BookRepository>();
            // services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IHirehubRepository, HirehubRepository>();
            /*  services.AddSession(options =>
              {
                  options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout duration
                  options.Cookie.HttpOnly = true;
                  options.Cookie.IsEssential = true;
              });*/
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./v1/swagger.json", "My API V1"); //originally "./swagger/v1/swagger.json"
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}