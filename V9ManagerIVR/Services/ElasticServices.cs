using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Extension;

namespace V9ManagerIVR.Services
{
    public interface IElasticServices
    {
        //Task<(bool, List<CustomerIdentityPhoneModel>)> GetByPhone(string phone);
        Task<long> ExtensionCountAsync();
        Task<bool> BulkUpsertListExtension(List<ExtensionElasticModel> extensions);
        Task<bool> DeleteExtensionManyAsync(Guid[] extensions);

        Task UpdateExtensionAsync(ExtensionElasticModel extension);

        Task CreateExtensionAsync(ExtensionElasticModel extension);

        Task<bool> SaveManyExtensionAsync(ExtensionElasticModel[] extensions);

        Task<bool> SaveBulkExtensionAsync(ExtensionElasticModel[] extensions);

        /// <summary>
        /// Kiểm tra id extension tồn tại
        /// </summary>
        /// <param name="extensionId"></param>
        /// <returns></returns>
        Task<bool> CheckExitstById(Guid extensionId);
    }


    public class ElasticServices : IElasticServices
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticServices> _logger;
        private readonly IConfiguration _Configuration;

        public ElasticServices(IElasticClient elasticClient, ILogger<ElasticServices> logger, IConfiguration Configuration)
        {
            _Configuration = Configuration;
            _elasticClient = elasticClient;
            _logger = logger;
        }

        /// <summary>
        /// Kiểm tra id extension tồn tại
        /// </summary>
        /// <param name="extensionId"></param>
        /// <returns></returns>
        public async Task<bool> CheckExitstById(Guid extensionId)
        {
            var response = await _elasticClient.SearchAsync<ExtensionElasticModel>(s => s
                 .Query(q => q
                     .Term(p => p.Id, extensionId)
                 )
             );
            return response.Documents != null && response.Documents.Count > 0;
            //var response = await _elasticClient.SearchAsync<Extension>(s => s
            //           .Query(q => q
            //                .Match(m => m
            //                    .Field(f => f.Id)
            //            .Query(extensionId.ToString())
            //        )).Explain());

        }

        public async Task<long> ExtensionCountAsync()
        {
            var response = await _elasticClient.CountAsync<ExtensionElasticModel>(x =>
                x.Index($"{_Configuration["elasticsearch:index"]}-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}"));
            return response.Count;
        }

        public async Task<bool> BulkUpsertListExtension(List<ExtensionElasticModel> extensions)
        {
            try
            {
                var indexx = $"{_Configuration["elasticsearch:index"]}-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}";
                var asyncBulkIndexResponse = await _elasticClient.BulkAsync(b => b
                     .UpdateMany<ExtensionElasticModel, object>(extensions, (bu, doc) =>
                     {
                         bu.Index(indexx);
                         bu.Id(doc.Id);
                         bu.Doc(doc);
                         bu.DocAsUpsert(true);
                         return bu;
                     })
                     .Refresh(Refresh.WaitFor));
                if (!asyncBulkIndexResponse.IsValid) return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

        }

        public async Task<bool> DeleteExtensionManyAsync(Guid[] extensions)
        {
            try
            {
                await _elasticClient.DeleteByQueryAsync<ExtensionElasticModel>(x => x.Query(s => s.Ids(i => i.Values(extensions))));
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }

        }

        public async Task UpdateExtensionAsync(ExtensionElasticModel extension)
        {
            var response = await _elasticClient.UpdateAsync<ExtensionElasticModel>(extension, u => u.Doc(extension));
            if (!response.IsValid)
            {
                var message = $"UpdateExtensionAsync: {response.Id}, {response.OriginalException}, {response.Result}";
                _logger.LogError(message);
            }
        }

        public async Task CreateExtensionAsync(ExtensionElasticModel extension)
        {
            var response = await _elasticClient.IndexDocumentAsync<ExtensionElasticModel>(extension);
            if (!response.IsValid)
            {
                var message = $"CreateExtensionAsync: {response.Id}, {response.OriginalException}, {response.Result}";
                _logger.LogError(message);
            }
        }

        public async Task<bool> SaveManyExtensionAsync(ExtensionElasticModel[] extensions)
        {
            var result = await _elasticClient.IndexManyAsync<ExtensionElasticModel>(extensions);
            if (result.Errors)
            {
                List<string> ids = new();
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index Extension document Id {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                    ids.Add(itemWithError.Id);
                }
                return false;
            }
            return true;
        }

        public async Task<bool> SaveBulkExtensionAsync(ExtensionElasticModel[] extensions)
        {
            var indexx = $"{_Configuration["elasticsearch:index"]}-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower()}";
            var result = await _elasticClient.BulkAsync(b => b.Index(indexx)
                .IndexMany(extensions));
            if (result.Errors)
            {
                List<string> ids = new();
                // the response can be inspected for errors
                foreach (var itemWithError in result.ItemsWithErrors)
                {
                    _logger.LogError("Failed to index document {0}: {1}",
                        itemWithError.Id, itemWithError.Error);
                    ids.Add(itemWithError.Id);
                }
                return false;
            }
            return true;
        }
    }

}
