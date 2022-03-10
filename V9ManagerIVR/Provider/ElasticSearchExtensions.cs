using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using V9ManagerIVR.Models.Extension;

namespace V9ManagerIVR.Provider
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["elasticsearch:url"];
            var defaultIndex = $"{configuration["elasticsearch:index"]}-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}";

            var settings = new ConnectionSettings(new Uri(url)).DefaultIndex(defaultIndex);

            //AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }

        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            //settings.DefaultMappingFor<ExtensionElasticModel>(m => m.Ignore(p => p.).Ignore(x => x.UpdatedBy));
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName,
                index => index.Map<ExtensionElasticModel>(x => x.AutoMap())
            );
        }
    }
}
