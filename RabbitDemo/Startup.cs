using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitDemo.Consumer;
using System;

namespace RabbitDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddMassTransit(x =>
            {
                
                x.AddConsumer<InfoReceivedConsumer>();
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cur =>
                {
                    cur.UseHealthCheck(provider);
                    cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cur.ReceiveEndpoint("CreateInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<InfoReceivedConsumer>(provider);
                    });
                    cur.ReceiveEndpoint("GetIdInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<InfoReceivedConsumer>(provider);
                    });
                    cur.ReceiveEndpoint("GetAllInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<InfoReceivedConsumer>(provider);
                    });
                    cur.ReceiveEndpoint("UpdateContactInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<InfoReceivedConsumer>(provider);
                    });
                    cur.ReceiveEndpoint("DeleteInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.ConfigureConsumer<InfoReceivedConsumer>(provider);
                    });
                }));
            });*/
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order.Microservice", Version = "v1" });
            });
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order.Microservice v1"));
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
