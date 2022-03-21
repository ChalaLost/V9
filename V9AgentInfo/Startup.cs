using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using V9AgentInfo.DataAccess.EF;
using V9AgentInfo.Hubs;

namespace V9AgentInfo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.UseHealthCheck(provider);
                    config.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                }));
            });
            services.AddControllers().AddNewtonsoftJson();

            services.AddMassTransitHostedService();

            services.AddCors(option => option.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:19401");
            }));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR(cfg => cfg.EnableDetailedErrors = true);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "V9AgentInfo", Version = "v1" });
            });
            services.AddEFConfiguration(Configuration);

            var setting = new ConnectionSettings();
            services.AddSingleton<IElasticClient>(new ElasticClient(setting));
            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "V9AgentInfo v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            /*app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());*/
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalR>("/signalr");
                endpoints.MapControllers();
            });
        }
        private void JsonOptions(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        }
    }
}
