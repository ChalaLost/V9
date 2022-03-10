using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.Entities.V9Role;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.DataAccess.EF
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
            var mySQLRoleConnectionString = configuration.GetConnectionString("RoleConnection");
            services.AddDbContext<RoleContext>(options =>
            {
                options.UseLazyLoadingProxies().UseMySql(mySQLRoleConnectionString, ServerVersion.AutoDetect(mySQLRoleConnectionString));
            });

            services.AddTransient<IIVRServices, IVRServices>();
            services.AddTransient<ICalendarServices, CalendarServices>();
            services.AddTransient<IExtensionServices, ExtensionServices>();
            services.AddTransient<IIntergrationServices, IntergrationServices>();
            services.AddTransient<IScriptServices, ScriptServices>();
            services.AddTransient<IIVRRecordFileServices, IVRRecordFileServices>();
            services.AddTransient<IQueueServices, QueueServices>();

            return services;
        }
    }
}
