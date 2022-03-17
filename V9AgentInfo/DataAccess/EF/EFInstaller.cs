using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Services;

namespace V9AgentInfo.DataAccess.EF
{
    public static class EFInstaller
    {
        public static IServiceCollection AddEFConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var mySQLConnectionString = configuration.GetConnectionString("DBConnection");
            services.AddDbContext<V9Context>(options =>
            {
                options.UseLazyLoadingProxies().UseMySql(mySQLConnectionString, ServerVersion.AutoDetect(mySQLConnectionString));
            });



            services.AddTransient<IInfoServices, Infoservices>();
            services.AddTransient<INotifyServices, Notifyservices>();

            return services;
        }
    }
}
