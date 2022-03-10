using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using V9Common;
using V9ManagerIVR.Models.Company;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Services
{
    public interface IIVRRecordFileServices
    {
        /// <summary>
        /// Danh sách file IVR của công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DataTableResponseModel> GetRecordFiles(DataTableAjaxPostModel model);
        /// <summary>
        /// Xóa file thu âm IVR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task DeleteIVRRecordFileCompany(Guid id, DeleteIVRRecordModel model, string userName);
        /// Tạo mới file thu âm IVR
        /// </summary>
        /// <param name="model"></param>
        /// <param name="realFileName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task CreateIVRRecordFileCompany(CreateIVRFileCompanyModel model, string realFileName, string userName);
        /// <summary>
        /// Cập nhật file thu âm IVR công ty
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task UpdateIVRRecordFileCompany(UpdateIVRFileCompanyModel model, string realFileName, string userName);
    }
    public class IVRRecordFileServices : IIVRRecordFileServices
    {
        private readonly V9Context _Context;
        private readonly ServicesMapModel _ServicesMap;
        private readonly ILogger<IVRRecordFileServices> _logger;
        public IVRRecordFileServices(V9Context Context, IOptions<ServicesMapModel> ServicesMap, ILogger<IVRRecordFileServices> logger)
        {
            _Context = Context;
            _ServicesMap = ServicesMap.Value;
            _logger = logger;
        }

        /// <summary>
        /// Danh sách file IVR của công ty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<DataTableResponseModel> GetRecordFiles(DataTableAjaxPostModel model)
        {
            //Xử lý tìm kiếm
            var searchBy = model.Search;
            var packageId = model.GuidValue;
            //Xử lý paging
            var take = model.PageSize;
            var skip = (model.PageNumber - 1) * model.PageSize;

            var query = _Context.DefaultRecords.Where(x => x.CompanyId == model.GuidValue);
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(x => x.Name.Contains(searchBy) || x.Content.Contains(searchBy));

            var result = await query.Select(s => new ListRecordFileModel
            {
                Id = s.Id,
                Content = s.Content,
                CreatedBy = s.CreatedBy,
                CreatedDate = s.CreatedDate,
                CreateType = s.CreateType,
                Name = s.Name,
                FileName = s.FileName
            })
                .OrderByDescending(x => x.CreatedDate)
                .Skip(skip)
                .Take(take)
                .ToListAsync();


            //Xử lý index
            return new DataTableResponseModel()
            {
                Data = result ?? new List<ListRecordFileModel>(),
                RecordsTotal = await query.CountAsync()
            };
        }

        /// <summary>
        /// Xóa file thu âm IVR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task DeleteIVRRecordFileCompany(Guid id, DeleteIVRRecordModel model, string userName)
        {
            var item = await _Context.DefaultRecords.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (item != null)
            {
                item.UpdatedBy = userName;
                item.UpdatedDate = DateTime.Now;
                item.IsDeleted = true;

                //Xóa trên asterisk
                model.FileName = item.Name;
                await DeleteIVRRecordAsteriskCompany(model);

                await _Context.SaveChangesAsync();
            }

        }

        /// <summary>
        /// Xóa file trên asterisk
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        async Task<bool> DeleteIVRRecordAsteriskCompany(DeleteIVRRecordModel model)
        {
            try
            {
                var client = new RestClient($"{_ServicesMap.V9AsteriskConnect}/api/Company/DeleteIVRRecord");
                RestRequest request = new();
                request.Method = Method.POST;
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(model);
                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == HttpStatusCode.OK) return true;
                else return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Tạo mới file thu âm IVR
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task CreateIVRRecordFileCompany(CreateIVRFileCompanyModel model, string realFileName, string userName)
        {
            var item = new DefaultRecord()
            {
                CompanyId = model.CompanyId,
                Content = model.Content,
                FileName = model.FileName,
                RealFileName = realFileName,
                CreatedBy = userName,
                CreatedDate = DateTime.Now,
                CreateType = CreateTypeRecord.Upload,
                Id = Guid.NewGuid(),
                IsDeleted = false,
                Name = model.Name,
            };

            await _Context.DefaultRecords.AddAsync(item);
            await _Context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật file thu âm IVR công ty
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task UpdateIVRRecordFileCompany(UpdateIVRFileCompanyModel model, string realFileName, string userName)
        {
            var item = await _Context.DefaultRecords.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (item != null)
            {
                item.UpdatedBy = userName;
                item.UpdatedDate = DateTime.Now;

                item.Name = model.Name;
                item.FileName = model.FileName;
                item.Content = model.Content;
                item.RealFileName = realFileName;

                await _Context.SaveChangesAsync();
            }
        }
    }
}
