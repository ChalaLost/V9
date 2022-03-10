using System;
using System.Collections.Generic;
using System.Globalization;
using V9Common;

namespace V9ManagerIVR.Models.Calendar
{
    public class CalendarCreateModel
    {
        public string Name { get; set; }
        /// <summary>
        /// Loại lịch ivr
        /// theo thứ hoặc theo ngày
        /// </summary>
        public V9_ScheduleActionType CalendarType { get; set; }
        /// <summary>
        /// Nếu là lịch thứ
        /// </summary>
        public List<CalendarDayInWeekCreateModel> CalendarDayInWeeks { get; set; }
        /// <summary>
        /// Nếu là lịch ngày
        /// </summary>
        public List<CalendarDateCreateModel> CalendarDates { get; set; }
    }

    public class CalendarUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Loại lịch ivr
        /// theo thứ hoặc theo ngày
        /// </summary>
        public V9_ScheduleActionType CalendarType { get; set; }
        /// <summary>
        /// Nếu là lịch thứ
        /// </summary>
        public List<CalendarDayInWeekCreateModel> CalendarDayInWeeks { get; set; }
        /// <summary>
        /// Nếu là lịch ngày
        /// </summary>
        public List<CalendarDateCreateModel> CalendarDates { get; set; }
    }


    #region Lịch thứ

    public class CalendarDayInWeekCreateModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<CalendarDayInWeekTimeCreateModel> Times { get; set; }
    }

    public class CalendarDayInWeekTimeCreateModel
    {
        public string FromTime { get; set; }
        public TimeSpan? FromTimeData
        {
            get
            {
                if (!string.IsNullOrEmpty(FromTime) && TimeSpan.TryParseExact(FromTime, @"hh\:mm\:ss", CultureInfo.CurrentCulture, out TimeSpan span))
                    return span;
                return null;
            }
        }
        public string ToTime { get; set; }
        public TimeSpan? ToTimeData
        {
            get
            {
                if (!string.IsNullOrEmpty(ToTime) && TimeSpan.TryParseExact(ToTime, @"hh\:mm\:ss", CultureInfo.CurrentCulture, out TimeSpan span))
                    return span;
                return null;
            }
        }
    }

    #endregion

    #region Lịch ngày

    public class CalendarDateCreateModel
    {
        public string FromDate { get; set; }
        public DateTime? FromDateData
        {
            get
            {
                if (!string.IsNullOrEmpty(FromDate) &&
                    DateTime.TryParseExact(FromDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    return date;
                return null;
            }
        }
        public string ToDate { get; set; }
        public DateTime? ToDateData
        {
            get
            {
                if (!string.IsNullOrEmpty(ToDate) &&
                    DateTime.TryParseExact(ToDate, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    return date;
                return null;
            }
        }

        public DayOfWeek DayOfWeek { get => FromDateData.HasValue ? FromDateData.Value.DayOfWeek : DayOfWeek.Sunday; }
        public TimeSpan? FromTime { get => FromDateData.HasValue ? FromDateData.Value.TimeOfDay : TimeSpan.Zero; }
        public TimeSpan? ToTime { get => ToDateData.HasValue ? ToDateData.Value.TimeOfDay : TimeSpan.Zero; }

    }

    #endregion



    #region Validate

    public class CalendarDayInWeekValidateModel
    {
        public string CalendarName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<CalendarDayInWeekTimevalidateModel> Times { get; set; }
    }

    public class CalendarDayInWeekTimevalidateModel
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    public class CalendarDateValidateModel
    {
        public string CalendarName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return FromDate.DayOfWeek;
            }
        }
        public TimeSpan FromTime { get => FromDate.TimeOfDay; }
        public TimeSpan ToTime { get => ToDate.TimeOfDay; }
    }

    #endregion
}
