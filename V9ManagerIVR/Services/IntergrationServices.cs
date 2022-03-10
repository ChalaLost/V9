using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Company;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;

namespace V9ManagerIVR.Services
{
    public interface IIntergrationServices
    {
        Task<ConnectionDatabaseModel> GetConectionDatabaseInfo(Guid companyid);
        /// <summary>
        /// Chuyển tiếp file đến asterisk
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        Task<bool> TransitRecordFileIVRCompany(TransitRecordFileIVRCompanyModel model, string pathFile);

    }


    public class IntergrationServices : IIntergrationServices
    {
        private readonly V9Context _Context;
        private readonly ServicesMapModel _ServicesMap;
        private readonly ILogger<IntergrationServices> _logger;
        public IntergrationServices(V9Context Context, IOptions<ServicesMapModel> ServicesMap, ILogger<IntergrationServices> logger)
        {
            _Context = Context;
            _ServicesMap = ServicesMap.Value;
            _logger = logger;
        }

        public async Task<ConnectionDatabaseModel> GetConectionDatabaseInfo(Guid companyid)
        {
            try
            {
                var client = new RestClient($"{_ServicesMap.V9ManagerAPI}/api/Intergration/GetDatabaseInfoByCompany/" + companyid);
                client.Authenticator = new HttpBasicAuthenticator("admin", "123@123aA");
                RestRequest request = new();
                request.Method = Method.GET;
                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Two ways to get the result:
                    string rawResponse = response.Content;
                    var myClass = JsonConvert.DeserializeObject<GetDatabaseInfoByCompanyResponseModel>(rawResponse);
                    return myClass.Result ? myClass.Info : null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Chuyển tiếp file đến asterisk
        /// </summary>
        /// <param name="model"></param>
        /// <param name="pathFile"></param>
        /// <returns></returns>
        public async Task<bool> TransitRecordFileIVRCompany(TransitRecordFileIVRCompanyModel model, string pathFile)
        {
            try
            {
                var client = new RestClient($"{_ServicesMap.V9AsteriskConnect}/api/Company/TransitRecordFileIVRCompany");
                var request = new RestRequest(Method.POST);
                request.AlwaysMultipartFormData = true;
                request.AddFile("File", pathFile);
                request.AddParameter("File", pathFile);
                request.AddParameter("SSH_IP", model.SSH_IP);
                request.AddParameter("SSH_UserName", model.SSH_UserName);
                request.AddParameter("SSH_Password", model.SSH_Password);
                request.AddParameter("CompanyCode", model.CompanyCode);
                var response = await client.ExecuteAsync(request);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return false;
        }

    }
}
