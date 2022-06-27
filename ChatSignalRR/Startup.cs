using AutoMapper;
using ChatSignalRR.Data;
using ChatSignalRR.DataAccess.EF;
using ChatSignalRR.IndentityServer;
using ChatSignalRR.Models;
using ChatSignalRR.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;


namespace ChatSignalRR
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
            
            services.AddIdentity<ApplicationUser, IdentityRole>() // để cho nó dùng được UserManger và roleManager
                .AddEntityFrameworkStores<ChatContext>()
                .AddDefaultTokenProviders();
            // add IdentiyServer
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

            }) 
          .AddInMemoryApiResources(Config.Apis) // bên folder IdentityServer thêm Config
          .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
          .AddInMemoryClients(Config.Clients) // lấy ra các client
          .AddInMemoryIdentityResources(Config.Ids)

          .AddInMemoryApiScopes(Config.ApiScopes)
          .AddAspNetIdentity<ApplicationUser>()
          .AddDeveloperSigningCredential();

            services.AddTransient<IEmailSender, EmailSenderService>();
            services.AddAutoMapper(typeof(Startup));

            services.AddEFConfiguration(Configuration);

            services.AddAuthentication()
             .AddLocalApi("Bearer", option =>
             {
                 option.ExpectedScope = "api.WebApp";
             });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>  // thêm một cái chính sách
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });


            IMvcBuilder build =  services.AddRazorPages(options =>
            {
                options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var attributeRouteModel = selector.AttributeRouteModel;
                        attributeRouteModel.Order = -1;
                        attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                    }
                });
            });

#if DEBUG
            var env = Environment.GetEnvironmentVariable("ASPNETCORE-ENVIROMENT");
            if(env == Environments.Development)
            {
                build.AddRazorRuntimeCompilation();
            }
#endif

            services.AddControllersWithViews();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatTest", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["AuthorityUrl"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "api.WebApp", "WebApp API" } }
                        },
                    },
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "api.WebApp" }
                    }
                });


            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatTest V1");
            });

        }
    }
}
