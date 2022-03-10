using System;
using System.Collections.Generic;
using V9Common;

namespace V9ManagerIVR.Models.Calendar
{
    public class CalendarDetailModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public V9_ScheduleActionType CalendarType { get; set; }
        /// <summary>
        /// Nếu là lịch thứ
        /// </summary>
        public List<CalendarDetailDayInWeekModel> CalendarDayInWeeks { get; set; }
        /// <summary>
        /// Nếu là lịch ngày
        /// </summary>
        public List<CalendarDetailDateModel> CalendarDates { get; set; }
    }

    public class CalendarDetailDayInWeekModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public List<CalendarDetailDayInWeekTimeModel> Times = new();
    }

    public class CalendarDetailDayInWeekTimeModel
    {
        public TimeSpan FromTime { get; set; }
        public string FromTimeStr { get => FromTime.ToString(@"hh\:mm"); }
        public TimeSpan ToTime { get; set; }
        public string ToTimeStr { get => ToTime.ToString(@"hh\:mm"); }
    }


    public class CalendarDetailDateModel
    {
        public DateTime FromDate { get; set; }
        public string FromDateStr { get => FromDate.ToString("dd-MM-yyyy HH:mm"); }
        public DateTime ToDate { get; set; }
        public string ToDateStr { get => ToDate.ToString("dd-MM-yyyy HH:mm"); }
    }


}
