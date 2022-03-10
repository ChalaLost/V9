using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Steeltoe.Discovery.Client;
using V9ManagerIVR.DataAccess.EF;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR
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
            services.AddAutoMapper(typeof(Startup));
            services.AddDiscoveryClient(Configuration);
            services.AddControllers().AddNewtonsoftJson(JsonOptions);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ManagerIVR", Version = "v1" });
            });
            services.AddEFConfiguration(Configuration);
            services.Configure<ServicesMapModel>(Configuration.GetSection("ServicesMap"));

            #region Elastic
            services.AddSingleton<IElasticServices, ElasticServices>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddElasticsearch(Configuration);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ManagerIVR v1"));

            app.UseDiscoveryClient();
            app.UseEndpoints(endpoints =>
            {
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
