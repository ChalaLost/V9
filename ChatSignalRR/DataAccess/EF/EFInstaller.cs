using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatSignalRR.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using ChatSignalRR.Services;

namespace ChatSignalRR.DataAccess.EF
{
    public static class EFInstaller
    {
        public static IServiceCollection AddEFConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var mySQLConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ChatContext>(options =>
            {
                options.UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString));
            });
            services.AddTransient<IEmailSender, EmailSenderService>();

            return services;
        }
    }
}
