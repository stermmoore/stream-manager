using System;
using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StreamManager.Repositories;

namespace StreamManager
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
            services.AddControllers();

            services.AddHealthChecks();

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "Stream Manager API", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration);

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();


            if (GetRepositoryType() == RepositoryType.InMemory)
                services.AddSingleton<IUserStreamRepository, InMemoryUserStreamRepository>();
            else
                services.AddTransient<IUserStreamRepository, DynamoDbUserStreamReporitory>();
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            app.UseSwagger();

            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "stream-manager");
            });
        }

        private RepositoryType GetRepositoryType()
        {
            RepositoryType repositoryType;

            if(Enum.TryParse(Configuration["RepositoryType"], true, out repositoryType))
                return repositoryType;

            throw new ApplicationException($"Repository Type: {Configuration["RepositoryType"]} is invalid");
        }
    }
}
