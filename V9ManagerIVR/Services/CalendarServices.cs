using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Calendar;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Services
{
    public interface ICalendarServices
    {
        /// <summary>
        /// Tạo lịch
        /// </summary>
        Task Create(CalendarCreateModel model, string userName, Guid companyId);
        /// <summary>
        /// Validate tạo lịch
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<(bool, string)> ValidateCreate(CalendarCreateModel model, Guid company);
        /// <summary>
        /// Validate cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        Task<(bool, string)> ValidateUpdate(CalendarUpdateModel model, Guid company);
        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task Update(CalendarUpdateModel model, string userName, Guid companyId);
        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task Delete(Guid id, Guid companyId, string userName);
        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<DataTableResponseModel> GetAll(DataTableAjaxPostModel model, Guid companyId);
        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<CalendarDetailModel> GetById(Guid id, Guid companyId);
    }

    public class CalendarServices : ICalendarServices
    {
        private readonly V9Context _Context;
        public CalendarServices(V9Context context)
        {
            _Context = context;
        }

        /// <summary>
        /// Validate tạo lịch
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<(bool, string)> ValidateCreate(CalendarCreateModel model, Guid company)
        {
            switch (model.CalendarType)
            {
                case V9Common.V9_ScheduleActionType.DayOfWeek:
                    {
                        //Kiểm tra với lịch thứ
                        var check = await ValidateCreateDayOfWeekWithDayOfWeek(model.CalendarDayInWeeks, company);
                        if (!check.Item1) return (check);

                        //Kiểm tra lịch ngày
                        check = await ValidateCreateDateWithDayOfWeek(model.CalendarDayInWeeks, company);
                        if (!check.Item1) return (check);

                    }
                    break;
                case V9Common.V9_ScheduleActionType.Date:
                    {
                        //Kiểm tra lịch thứ
                        var check = await ValidateCreateDayOfWeekWithDate(model.CalendarDates, company);
                        if (!check.Item1) return (check);

                        //Kiểm tra lịch ngày
                        check = await ValidateCreateDateWithDate(model.CalendarDates, company);
                        if (!check.Item1) return (check);
                    }
                    break;
                default:
                    break;
            }

            return (true, string.Empty);
        }

        #region Sub validate create

        /// <summary>
        /// Validate Lịch thứ với model là lịch thứ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateCreateDayOfWeekWithDayOfWeek(List<CalendarDayInWeekCreateModel> model, Guid companyId)
        {
            var items = await _Context.CalendarDayInWeeks.Where(x => x.Calendar.CompanyId == companyId)
                          .Select(s => new CalendarDayInWeekValidateModel
                          {
                              CalendarName = s.Calendar.Name,
                              DayOfWeek = s.DayOfWeek,
                              Times = s.Times.Select(s1 => new CalendarDayInWeekTimevalidateModel
                              {
                                  FromTime = s1.FromTime,
                                  ToTime = s1.ToTime
                              }).ToList()
                          }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).SelectMany(s => s.Times).ToList();
                    foreach (var mTime in modelTimes)
                    {
                        if (item.Times.Any(x => (mTime.FromTimeData.Value >= x.FromTime && mTime.FromTimeData.Value <= x.ToTime)
                            || mTime.ToTimeData.Value >= x.FromTime && mTime.ToTimeData <= x.ToTime))
                        {
                            return (false, $"Trùng lịch với: {item.CalendarName}");
                        }
                    }
                }
            }

            return (true, string.Empty);
        }
        /// <summary>
        /// Validate Lịch ngày với model là lịch thứ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateCreateDateWithDayOfWeek(List<CalendarDayInWeekCreateModel> model, Guid companyId)
        {
            //Cần convert ra thứ để so sánh
            var items = await _Context.CalendarDates.Where(x => x.Calendar.CompanyId == companyId)
                .Select(s => new CalendarDateValidateModel
                {
                    CalendarName = s.Calendar.Name,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate
                }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).SelectMany(s => s.Times).ToList();
                    if (modelTimes.Any(x => (item.FromTime >= x.FromTimeData.Value && item.FromTime <= x.ToTimeData.Value)
                         || (item.ToTime <= x.ToTimeData.Value && item.ToTime >= x.FromTimeData.Value)))
                    {
                        return (false, $"Trùng lịch với: {item.CalendarName}");
                    }
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validate lịch thứ với model là lịch ngày
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateCreateDayOfWeekWithDate(List<CalendarDateCreateModel> model, Guid companyId)
        {
            var items = await _Context.CalendarDayInWeeks.Where(x => x.Calendar.CompanyId == companyId)
                          .Select(s => new CalendarDayInWeekValidateModel
                          {
                              CalendarName = s.Calendar.Name,
                              DayOfWeek = s.DayOfWeek,
                              Times = s.Times.Select(s1 => new CalendarDayInWeekTimevalidateModel
                              {
                                  FromTime = s1.FromTime,
                                  ToTime = s1.ToTime
                              }).ToList()
                          }).ToListAsync();

            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).ToList();
                    foreach (var mTime in modelTimes)
                    {
                        if (item.Times.Any(x => (mTime.FromTime.Value >= x.FromTime && mTime.FromTime.Value <= x.ToTime)
                            || mTime.ToTime.Value >= x.FromTime && mTime.ToTime <= x.ToTime))
                        {
                            return (false, $"Trùng lịch với: {item.CalendarName}");
                        }
                    }
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validate lịch ngày với model là lịch ngày
        /// </summary>
        /// <param name="model"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateCreateDateWithDate(List<CalendarDateCreateModel> model, Guid companyId)
        {
            //Cần convert ra thứ để so sánh
            var items = await _Context.CalendarDates.Where(x => x.Calendar.CompanyId == companyId)
                .Select(s => new CalendarDateValidateModel
                {
                    CalendarName = s.Calendar.Name,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate
                }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).ToList();
                    if (modelTimes.Any(x => (item.FromTime >= x.FromTime.Value && item.FromTime <= x.ToTime.Value)
                         || (item.ToTime <= x.ToTime.Value && item.ToTime >= x.FromTime.Value)))
                    {
                        return (false, $"Trùng lịch với: {item.CalendarName}");
                    }
                }
            }

            return (true, string.Empty);
        }

        #endregion

        #region Sub validate update

        /// <summary>
        /// Validate Lịch thứ với model là lịch thứ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="calendarId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateUpdateDayOfWeekWithDayOfWeek(List<CalendarDayInWeekCreateModel> model, Guid calendarId, Guid companyId)
        {
            var items = await _Context.CalendarDayInWeeks.Where(x => x.Calendar.Id != calendarId && x.Calendar.CompanyId == companyId)
                          .Select(s => new CalendarDayInWeekValidateModel
                          {
                              CalendarName = s.Calendar.Name,
                              DayOfWeek = s.DayOfWeek,
                              Times = s.Times.Select(s1 => new CalendarDayInWeekTimevalidateModel
                              {
                                  FromTime = s1.FromTime,
                                  ToTime = s1.ToTime
                              }).ToList()
                          }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).SelectMany(s => s.Times).ToList();
                    foreach (var mTime in modelTimes)
                    {
                        if (item.Times.Any(x => (mTime.FromTimeData.Value >= x.FromTime && mTime.FromTimeData.Value <= x.ToTime)
                            || mTime.ToTimeData.Value >= x.FromTime && mTime.ToTimeData <= x.ToTime))
                        {
                            return (false, $"Trùng lịch với: {item.CalendarName}");
                        }
                    }
                }
            }

            return (true, string.Empty);
        }
        /// <summary>
        /// Validate Lịch ngày với model là lịch thứ
        /// </summary>
        /// <param name="model"></param>
        /// <param name="calendarId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateUpdateDateWithDayOfWeek(List<CalendarDayInWeekCreateModel> model, Guid calendarId, Guid companyId)
        {
            //Cần convert ra thứ để so sánh
            var items = await _Context.CalendarDates.Where(x => x.Calendar.Id != calendarId && x.Calendar.CompanyId == companyId)
                .Select(s => new CalendarDateValidateModel
                {
                    CalendarName = s.Calendar.Name,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate
                }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).SelectMany(s => s.Times).ToList();
                    if (modelTimes.Any(x => (item.FromTime >= x.FromTimeData.Value && item.FromTime <= x.ToTimeData.Value)
                         || (item.ToTime <= x.ToTimeData.Value && item.ToTime >= x.FromTimeData.Value)))
                    {
                        return (false, $"Trùng lịch với: {item.CalendarName}");
                    }
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validate lịch thứ với model là lịch ngày
        /// </summary>
        /// <param name="model"></param>
        /// <param name="calendarId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateUpdateDayOfWeekWithDate(List<CalendarDateCreateModel> model, Guid calendarId, Guid companyId)
        {
            var items = await _Context.CalendarDayInWeeks.Where(x => x.Calendar.Id != calendarId && x.Calendar.CompanyId == companyId)
                          .Select(s => new CalendarDayInWeekValidateModel
                          {
                              CalendarName = s.Calendar.Name,
                              DayOfWeek = s.DayOfWeek,
                              Times = s.Times.Select(s1 => new CalendarDayInWeekTimevalidateModel
                              {
                                  FromTime = s1.FromTime,
                                  ToTime = s1.ToTime
                              }).ToList()
                          }).ToListAsync();

            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).ToList();
                    foreach (var mTime in modelTimes)
                    {
                        if (item.Times.Any(x => (mTime.FromTime.Value >= x.FromTime && mTime.FromTime.Value <= x.ToTime)
                            || mTime.ToTime.Value >= x.FromTime && mTime.ToTime <= x.ToTime))
                        {
                            return (false, $"Trùng lịch với: {item.CalendarName}");
                        }
                    }
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Validate lịch ngày với model là lịch ngày
        /// </summary>
        /// <param name="model"></param>
        /// <param name="calendarId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        async Task<(bool, string)> ValidateUpdateDateWithDate(List<CalendarDateCreateModel> model, Guid calendarId, Guid companyId)
        {
            //Cần convert ra thứ để so sánh
            var items = await _Context.CalendarDates.Where(x => x.Calendar.Id != calendarId && x.Calendar.CompanyId == companyId)
                .Select(s => new CalendarDateValidateModel
                {
                    CalendarName = s.Calendar.Name,
                    FromDate = s.FromDate,
                    ToDate = s.ToDate
                }).ToListAsync();
            if (items != null)
            {
                foreach (var item in items)
                {
                    var modelTimes = model.Where(x => x.DayOfWeek == item.DayOfWeek).ToList();
                    if (modelTimes.Any(x => (item.FromTime >= x.FromTime.Value && item.FromTime <= x.ToTime.Value)
                         || (item.ToTime <= x.ToTime.Value && item.ToTime >= x.FromTime.Value)))
                    {
                        return (false, $"Trùng lịch với: {item.CalendarName}");
                    }
                }
            }

            return (true, string.Empty);
        }
        #endregion

        /// <summary>
        /// Tạo lịch
        /// </summary>
        public async Task Create(CalendarCreateModel model, string userName, Guid companyId)
        {
            var item = new Calendar()
            {
                Id = Guid.NewGuid(),
                CalendarType = model.CalendarType,
                IsDeleted = false,
                CreatedBy = userName,
                CreatedDate = DateTime.Now,
                CompanyId = companyId,
                Name = model.Name
            };
            if (model.CalendarDayInWeeks != null && model.CalendarDayInWeeks.Count > 0)
            {
                item.CalendarDayInWeeks = model.CalendarDayInWeeks.Select(s => new CalendarDayInWeek
                {
                    DayOfWeek = s.DayOfWeek,
                    Times = s.Times.Select(s => new CalendarDayInWeekTime()
                    {
                        FromTime = s.FromTimeData.Value,
                        ToTime = s.ToTimeData.Value
                    }).ToList()
                }).ToList();
            }
            if (model.CalendarDates != null && model.CalendarDates.Count > 0)
            {
                item.CalendarDates = model.CalendarDates.Select(s => new CalendarDate()
                {
                    FromDate = s.FromDateData.Value,
                    ToDate = s.ToDateData.Value

                }).ToList();
            }


            await _Context.Calendars.AddAsync(item);
            await _Context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task Update(CalendarUpdateModel model, string userName, Guid companyId)
        {
            var item = await _Context.Calendars.Where(x => x.Id == model.Id && x.CompanyId == companyId).FirstOrDefaultAsync();
            if (item != null)
            {
                item.Name = model.Name;
                item.CalendarType = model.CalendarType;

                if (item.CalendarDayInWeeks != null && item.CalendarDayInWeeks.Count > 0)
                {
                    foreach (var item1 in item.CalendarDayInWeeks)
                    {
                        if (item1.Times != null && item1.Times.Count > 0)
                            _Context.CalendarDayInWeekTimes.RemoveRange(item1.Times);
                    }

                    _Context.CalendarDayInWeeks.RemoveRange(item.CalendarDayInWeeks);
                    await _Context.SaveChangesAsync();
                }
                if (item.CalendarDates != null && item.CalendarDates.Count > 0)
                {
                    _Context.CalendarDates.RemoveRange(item.CalendarDates);
                    await _Context.SaveChangesAsync();
                }

                item.CalendarDayInWeeks = model.CalendarDayInWeeks.Select(s => new CalendarDayInWeek
                {
                    DayOfWeek = s.DayOfWeek,
                    Times = s.Times.Select(s => new CalendarDayInWeekTime()
                    {
                        FromTime = s.FromTimeData.Value,
                        ToTime = s.ToTimeData.Value
                    }).ToList()
                }).ToList();
                item.CalendarDates = model.CalendarDates.Select(s => new CalendarDate()
                {
                    FromDate = s.FromDateData.Value,
                    ToDate = s.ToDateData.Value

                }).ToList();
                item.UpdatedBy = userName;
                item.UpdatedDate = DateTime.Now;

                await _Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Validate cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public async Task<(bool, string)> ValidateUpdate(CalendarUpdateModel model, Guid company)
        {
            switch (model.CalendarType)
            {
                case V9Common.V9_ScheduleActionType.DayOfWeek:
                    {
                        //Kiểm tra với lịch thứ
                        var check = await ValidateUpdateDayOfWeekWithDayOfWeek(model.CalendarDayInWeeks, model.Id, company);
                        if (!check.Item1) return (check);

                        //Kiểm tra lịch ngày
                        check = await ValidateUpdateDateWithDayOfWeek(model.CalendarDayInWeeks, model.Id, company);
                        if (!check.Item1) return (check);

                    }
                    break;
                case V9Common.V9_ScheduleActionType.Date:
                    {
                        //Kiểm tra lịch thứ
                        var check = await ValidateUpdateDayOfWeekWithDate(model.CalendarDates, model.Id, company);
                        if (!check.Item1) return (check);

                        //Kiểm tra lịch ngày
                        check = await ValidateUpdateDateWithDate(model.CalendarDates, model.Id, company);
                        if (!check.Item1) return (check);
                    }
                    break;
                default:
                    break;
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task Delete(Guid id, Guid companyId, string userName)
        {
            var item = await _Context.Calendars.Where(x => x.Id == id && x.CompanyId == companyId).FirstOrDefaultAsync();
            if (item != null)
            {
                item.UpdatedBy = userName;
                item.UpdatedDate = DateTime.Now;
                item.IsDeleted = true;

                //Thực hiện xóa các bản ghi liên quan
                var itemCalendar = await _Context.CalendarIVRs.Where(x => x.CalendarId == id).ToListAsync();
                if (itemCalendar != null)
                {
                    itemCalendar.ForEach(x => x.CalendarId = null);
                }

                await _Context.SaveChangesAsync();
            }
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

            var query = _Context.Calendars.Where(x => x.CompanyId == companyId);
            if (!string.IsNullOrEmpty(searchBy))
                query = query.Where(x => x.Name.Contains(searchBy));

            var result = await query.Select(s => new ListCalendarModel
            {
                Id = s.Id,
                Name = s.Name,
                CalendarType = s.CalendarType,
            })
                .OrderByDescending(x => x.Name)
                .Skip(skip)
                .Take(take)
                .ToListAsync();


            //Xử lý index
            return new DataTableResponseModel()
            {
                Data = result ?? new List<ListCalendarModel>(),
                RecordsTotal = await query.CountAsync()
            };
        }

        /// <summary>
        /// Lấy theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<CalendarDetailModel> GetById(Guid id, Guid companyId)
        {
            return await _Context.Calendars.Where(x => x.Id == id && x.CompanyId == companyId)
                  .Select(s => new CalendarDetailModel
                  {
                      Id = s.Id,
                      CalendarType = s.CalendarType,
                      Name = s.Name,
                      CalendarDates = s.CalendarDates.Select(x => new CalendarDetailDateModel()
                      {
                          FromDate = x.FromDate,
                          ToDate = x.ToDate
                      }).ToList(),
                      CalendarDayInWeeks = s.CalendarDayInWeeks.Select(x => new CalendarDetailDayInWeekModel
                      {
                          DayOfWeek = x.DayOfWeek,
                          Times = x.Times.Select(x1 => new CalendarDetailDayInWeekTimeModel
                          {
                              FromTime = x1.FromTime,
                              ToTime = x1.ToTime
                          }).ToList()
                      }).ToList()
                  }).FirstOrDefaultAsync();
        }

    }
}
