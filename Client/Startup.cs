using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using MassTransit;
using Client.Consumer;
using GreenPipes;
using V9AgentInfo.Services;
using V9AgentInfo.DataAccess.EF;
using V9AgentInfo.Hubs;

namespace Client
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
            services.AddMassTransit(x =>
            {

                x.AddConsumer<InfoReceivedConsumer>();
                x.AddConsumer<InfoReceivedConsumerList>();
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
                        oq.ConfigureConsumer<InfoReceivedConsumerList>(provider);
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
            });
            services.AddMassTransitHostedService();
            services.AddTransient<IInfoServices, Infoservices>();
            services.AddEFConfiguration(Configuration);
            services.AddCors(option => option.AddPolicy("CorsPolicy", builder =>
            {
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:19401");
            }));

            services.AddSignalR();
            services.AddControllersWithViews();

            services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                endpoints.MapHub<SignalR>("/signalr");
            });
        }
    }
}
