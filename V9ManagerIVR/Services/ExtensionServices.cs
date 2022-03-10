using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.Extension;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Services
{
    public interface IExtensionServices
    {

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<DataTableResponseModel> GetAll(DataTableAjaxPostModel model, Guid companyId);
        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> Update(UpdateExtensionModel model, Guid companyId, string userName, string subDomain);
        /// <summary>
        /// Kiểm tra cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool ValidateUpdate(UpdateExtensionModel model, out string message);
    }
    public class ExtensionServices : IExtensionServices
    {
        private readonly V9Context _Context;
        private readonly ILogger<ExtensionServices> _logger;
        private readonly IElasticServices _ElasticServices;
        private readonly IMapper _Mapper;

        public ExtensionServices(V9Context Context, ILogger<ExtensionServices> logger, IElasticServices ElasticServices, IMapper mapper)
        {
            _Mapper = mapper;
            _Context = Context;
            _logger = logger;
            _ElasticServices = ElasticServices;
        }


        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<DataTableResponseModel> GetAll(DataTableAjaxPostModel model, Guid companyId)
        {
            //Xử lý tìm kiếm
            var searchBy = model.Search;
            var packageId = model.GuidValue;
            //Xử lý paging
            var take = model.PageSize;
            var skip = (model.PageNumber - 1) * model.PageSize;

            var query = _Context.Extensions.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(x => x.Name.Contains(searchBy) || x.Exten.Contains(searchBy) || x.Network.Contains(searchBy));


            var allIVR = await _Context.IVRs.Where(x => x.CompanyId == companyId).Select(s => new SelectModel
            {
                Id = s.Id.ToString(),
                Name = s.Name
            }).ToListAsync();
            var allCalendar = await _Context.Calendars.Where(x => x.CompanyId == companyId).Select(s => new SelectModel
            {
                Id = s.Id.ToString(),
                Name = s.Name
            }).ToListAsync();

            var result = await query.Select(s => new ListExtensionModel
            {
                Id = s.Id,
                Name = s.Name,
                CurrentCall = s.CurrentCall,
                Exten = s.Exten,
                Network = s.Network,
                Details = s.CalendarIVRs.Select(x => new ListExtensionDetailModel
                {
                    Priority = x.Priority,
                    SelectedIVR = x.IVRId,
                    SelectedCalendar = x.CalendarId
                }).ToList()
            })
                .OrderByDescending(x => x.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            if (result != null && result.Count > 0)
            {
                result.ForEach(x =>
                {
                    if (x.Details.Count == 0) x.Details = new List<ListExtensionDetailModel>()
                    {
                        new ListExtensionDetailModel()
                        {
                            Calendars=allCalendar.Select(s1 => new SelectModel()
                            {
                                Id = s1.Id,
                                Name = s1.Name
                            }).ToList(),
                            IVRs =allIVR.Select(s1 => new SelectModel()
                            {
                                Id = s1.Id,
                                Name = s1.Name,
                            }).ToList()
                        }
                    };
                    else
                        x.Details.ForEach(s =>
                        {
                            s.Calendars = allCalendar.Select(s1 => new SelectModel()
                            {
                                Id = s1.Id,
                                Name = s1.Name,
                                Selected = s.SelectedCalendar.HasValue && s.SelectedCalendar.Value.ToString() == s1.Id
                            }).ToList();

                            s.IVRs = allIVR.Select(s1 => new SelectModel()
                            {
                                Id = s1.Id,
                                Name = s1.Name,
                                Selected = s.SelectedIVR.HasValue && s.SelectedIVR.Value.ToString() == s1.Id
                            }).ToList();
                        });
                });
            }

            //Xử lý index
            return new DataTableResponseModel()
            {
                Data = result ?? new List<ListExtensionModel>(),
                RecordsTotal = await query.CountAsync()
            };
        }

        /// <summary>
        /// Kiểm tra cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ValidateUpdate(UpdateExtensionModel model, out string message)
        {
            message = string.Empty;
            if (model.Details != null && model.Details.Count > 0)
            {
                if (model.Details.GroupBy(x => new { x.CalendarId, x.Priority }).Any(x => x.Any()))
                {
                    message = "Độ ưu tiên bị trùng";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> Update(UpdateExtensionModel model, Guid companyId, string userName, string subDomain)
        {
            try
            {
                var item = _Context.Extensions.Where(x => x.Id == model.Id && x.CompanyId == companyId).FirstOrDefault();
                if (item != null)
                {
                    item.CurrentCall = model.CurrentCall;
                    item.UpdatedBy = userName;
                    item.UpdatedDate = DateTime.Now;

                    #region Xóa đi 

                    if (item.CalendarIVRs != null && item.CalendarIVRs.Count > 0)
                    {
                        _Context.CalendarIVRs.RemoveRange(item.CalendarIVRs);
                        _Context.SaveChanges();
                    }

                    #endregion

                    #region Thêm mới lại

                    _Context.CalendarIVRs.AddRange(model.Details.Select(s => new CalendarIVR()
                    {
                        Id = Guid.NewGuid(),
                        IVRId = s.IVRId,
                        CalendarId = s.CalendarId,
                        Priority = s.Priority,
                        ExtensionId = item.Id
                    }).ToList());

                    #endregion

                    _Context.SaveChanges();

                    #region Xử lý đẩy sang elastic

                    //Đẩy sang elastic

                    var itemElastic = await _Context.Extensions.Include(x => x.CalendarIVRs).ThenInclude(x => x.IVR)
                        .Include(x => x.CalendarIVRs).ThenInclude(x => x.Calendar).ThenInclude(x => x.CalendarDayInWeeks).ThenInclude(x => x.Times)
                        .Include(x => x.CalendarIVRs).ThenInclude(x => x.Calendar).ThenInclude(x => x.CalendarDates)
                        .Where(x => x.Id == item.Id && x.CompanyId == companyId)
                        .Select(s => new ExtensionTempModel
                        {
                            Id = s.Id,
                            Exten = s.Exten,
                            CompanyId = s.CompanyId,
                            CalendarIVRs = s.CalendarIVRs.ToList()
                        }).FirstOrDefaultAsync();

                    //Lởm vãi
                    var myData = new ExtensionElasticModel()
                    {
                        Exten = item.Exten,
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        Subdomain = subDomain,

                        CalendarIVRs = itemElastic.CalendarIVRs.Select(s => new CalendarIVRElasticModel
                        {
                            Calendar = new CalendarElasticModel
                            {
                                CalendarType = s.Calendar.CalendarType,
                                CalendarDates = s.Calendar.CalendarDates.Select(s2 => new CalendarDateElasticModel
                                {
                                    FromDate = s2.FromDate,
                                    ToDate = s2.ToDate
                                }).ToList(),
                                CalendarDayInWeeks = s.Calendar.CalendarDayInWeeks.Select(s2 => new CalendarDayInWeekElasticModel
                                {
                                    DayOfWeek = s2.DayOfWeek,
                                    Times = s2.Times.Select(s3 => new CalendarDayInWeekTimeElasticModel
                                    {
                                        FromTime = s3.FromTime,
                                        ToTime = s3.ToTime
                                    }).ToList()
                                }).ToList()
                            },
                            Subdomain = subDomain,
                            Priority = s.Priority,
                            IVR = _Mapper.Map<IVRElasticModel>(s.IVR)
                        }).ToList()
                    };
                    var test = JsonConvert.SerializeObject(myData, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                    myData = JsonConvert.DeserializeObject<ExtensionElasticModel>(test);

                    var check = await _ElasticServices.BulkUpsertListExtension(new List<ExtensionElasticModel>() { myData });
                    if (!check) return false;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
            return true;
        }

    }
}
