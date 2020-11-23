using System.CodeDom.Compiler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scrutor;
using store_api.Objects;
using store_api.SqlServer.DAL;

namespace store_api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000",
                            "https://e-commerce-assignment-295115.ew.r.appspot.com");
                    });
            });
            services.AddSwaggerGen(c =>
            {
                c.UseOneOfForPolymorphism();
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "store-front-end",
                    Version = "0.1",
                    Description =
                        "API interface to handle the CRUD for the store",
                    Contact = new OpenApiContact
                    {
                        Name = "Harry Gillingham",
                        Email = "harrygillingham@hotmail.com"
                    },
                });

            });
            services.AddOpenApiDocument();

            ScanForAllRemainingRegistrations(services);

            AddConfigs(services);
        }

        public static void ScanForAllRemainingRegistrations(IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(Startup), typeof(Repository))
                .AddClasses(x => x.WithoutAttribute(typeof(GeneratedCodeAttribute)))
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime());
        }

        public void AddConfigs(IServiceCollection services)
        {
            services.Configure<ConnectionStrings>(option =>
                Configuration.GetSection(nameof(ConnectionStrings)).Bind(option));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "store-api");
            });


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
