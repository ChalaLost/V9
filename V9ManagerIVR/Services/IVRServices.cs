using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using V9Common;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Services
{
    public interface IIVRServices
    {
        Task<string> ValidateCreate(CreateIVRModel model);
        /// <summary>
        /// Tất cả theo công ty
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        //Task<IVR> All(Guid company);
        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        Task<IVR> GetById(Guid IVRId);
   

    }

    public class IVRServices : IIVRServices
    {
        private readonly V9Context _Context;
        private readonly ServicesMapModel _ServicesMap;
        private readonly ILogger<IVRServices> _logger;
        public IVRServices(V9Context Context, IOptions<ServicesMapModel> ServicesMap, ILogger<IVRServices> logger)
        {
            _Context = Context;
            _ServicesMap = ServicesMap.Value;
            _logger = logger;
        }


        /// <summary>
        /// Kiểm tra tồn tại IVR
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        //public async Task<bool> CheckExistById(Guid id, Guid company)
        //{
        //    return await _Context.IVRs.AnyAsync(x => x.Id == id && x.Company == company);
        //}

        /// <summary>
        /// Kiểm tra theo level 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="company"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        //public async Task<bool> CheckLevel(Guid id, Guid company, V9_IVRLevel type)
        //{
        //    return await _Context.IVRs.AnyAsync(x => x.Id == id && x.Level == type && x.Company == company);
        //}

        /// <summary>
        /// Kiểm tra khi khởi tạo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ValidateCreate(CreateIVRModel model)
        {
            //Cần kiểm tra tồn tại company

            switch (model.Level)
            {
                case V9_IVRLevel.LV1:
                    return "Không đúng định dạng cây thư mục";
                case V9_IVRLevel.LV2:
                    {

                    }
                    break;
                case V9_IVRLevel.LV3:
                    {
                        //if (!model.ParentId.HasValue || !await CheckExistById(model.ParentId.Value, model.Company))
                        //    return "Vui lòng chọn thư mục cha";

                        //if (model.Schedule == null || model.Action == null)
                        //    return "Vui lòng thiết lập lịch và hành động";
                        ////Kiểm tra lịch đúng định dạng
                        //var checkFormat = await ValidateCreateSchedule(model.Schedule);
                        //if (!string.IsNullOrEmpty(checkFormat)) return checkFormat;
                        ////Kiểm tra có đúng level cha là lv2 không
                        //if (!await CheckLevel(model.ParentId.Value, model.Company, V9_IVRLevel.LV2))
                        //    return "Không đúng thư mục cha";
                        ////Kiểm tra ngày loại trừ
                        //if (model.Schedule.ScheduleExcludes != null && model.Schedule.ScheduleExcludes.Count > 0)
                        //    if (!ValidateFormatCreateScheduleExclude(model.Schedule.ScheduleExcludes, out string message))
                        //        return message;

                    }
                    break;
                case V9_IVRLevel.LVn:
                    break;
                default:
                    break;
            }

            await Task.CompletedTask;
            return string.Empty;
        }

        /// <summary>
        /// Kiểm tra lịch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> ValidateCreateSchedule(CreateIVRScheduleModel model)
        {
            switch (model.Type)
            {
                case V9_ScheduleActionType.DayOfWeek:
                    {
                        //if (model. == null || model.ScheduleTimes.Count == 0)
                        //    return "Vui lòng chọn thời gian theo thứ";

                        //foreach (var item in model.ScheduleTimes)
                        //{
                        //    if (!DateTime.TryParseExact(item.FromTime, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                        //    {
                        //        string data = item.Day == System.DayOfWeek.Sunday ? "CN" : $"Thứ {item.Day + 1}";
                        //        return $"Sai định dạng thời gian, ngày {data}: {item.FromTime}";
                        //    }

                        //    if (!DateTime.TryParseExact(item.ToTime, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                        //    {
                        //        string data = item.Day == System.DayOfWeek.Sunday ? "CN" : $"Thứ {item.Day + 1}";
                        //        return $"Sai định dạng thời gian, ngày {data}: {item.ToTime}";
                        //    }
                        //}
                    }
                    break;
                case V9_ScheduleActionType.Date:
                    {
                        //foreach (var item in model.)
                        //{

                        //}
                        //if (!DateTime.TryParseExact(model.D_FromDate, FormatDate.DateTime_ddMMyyyyHHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                        //{
                        //    return $"Sai định dạng thời gian từ ngày, trong lịch ngày";
                        //}

                        //if (!DateTime.TryParseExact(model.D_ToDate, FormatDate.DateTime_ddMMyyyyHHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                        //{
                        //    return $"Sai định dạng thời gian đến ngày, trong lịch ngày";
                        //}
                    }
                    break;
                default:
                    break;
            }

            await Task.CompletedTask;

            return string.Empty;
        }

        /// <summary>
        /// Kiểm tra format ngày loại trừ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool ValidateFormatCreateScheduleExclude(List<CreateIVRScheduleExclude> model, out string message)
        {
            message = string.Empty;

            foreach (var item in model)
            {
                if (!DateTime.TryParseExact(item.Date, FormatDate.DateTime_103, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime _))
                {
                    message = $"Sai định dạng thời gian ngày loại trừ: {item.Date}";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Tất cả theo công ty
        /// </summary>
        /// <param name="company"></param>
        /// <returns></returns>
        //public async Task<IVR> All(Guid company)
        //{
        //    IVR item = new()
        //    {
        //        Name = "V9",
        //        Level = V9_IVRLevel.LV1,
        //        Childrens = await _Context.IVRs.Where(x => !x.ParentId.HasValue && x.Company == company).ToListAsync()
        //    };
        //    return item;
        //}

        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        public async Task<IVR> GetById(Guid IVRId)
        {
            return await _Context.IVRs.Where(x => x.Id == IVRId).FirstOrDefaultAsync();
        }

        
    }
}
