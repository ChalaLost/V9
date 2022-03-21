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
using RabbitMQ.Client;

namespace Client
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
                    var host = cur.Host(new Uri("rabbitmq://localhost"), h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cur.ReceiveEndpoint(host, "CreateInfo", e =>
                    {
                        e.BindMessageExchanges = false;
                        e.ConfigureConsumer<InfoReceivedConsumer>(provider);
                        e.Bind("Agent_Info", x =>
                        {
                            x.ExchangeType = ExchangeType.Topic;
                            x.RoutingKey = "info.topic.create";
                        });
                    });
                    cur.ReceiveEndpoint(host,"GetIdInfo", e =>
                    {
                        e.BindMessageExchanges = false;
                        e.ConfigureConsumer<InfoReceivedConsumer>(provider);
                        e.Bind("Agent_Info", x =>
                        {
                            x.ExchangeType = ExchangeType.Topic;
                            x.RoutingKey = "info.topic.getId";
                        });
                    });
                   /* cur.ReceiveEndpoint("GetAllInfo", oq =>
                    {
                        oq.UseMessageRetry(r => r.Interval(2, 100));
                        oq.Consumer<InfoReceivedConsumerList>(provider);
                    });*/
                   cur.ReceiveEndpoint(host, "GetAll_Info", e =>
                    {
                        e.BindMessageExchanges = false;
                        e.ConfigureConsumer<InfoReceivedConsumerList>(provider);
                        e.Bind("Agent_Info", x =>
                        {
                            x.ExchangeType = ExchangeType.Topic;
                            x.RoutingKey = "info.topic.getall";
                        });
                    });
                    cur.ReceiveEndpoint(host, "UpdateContactInfo", e =>
                    {
                        e.BindMessageExchanges = false;
                        e.ConfigureConsumer<InfoReceivedConsumer>(provider);
                        e.Bind("Agent_Info", x =>
                        {
                            x.ExchangeType = ExchangeType.Topic;
                            x.RoutingKey = "info.topic.updateContact";
                        });
                    });
                    cur.ReceiveEndpoint(host, "DeleteInfo", e =>
                    {
                        e.BindMessageExchanges = false;
                        e.ConfigureConsumer<InfoReceivedConsumer>(provider);
                        e.Bind("Agent_Info", x =>
                        {
                            x.ExchangeType = ExchangeType.Topic;
                            x.RoutingKey = "info.topic.delete";
                        });
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
