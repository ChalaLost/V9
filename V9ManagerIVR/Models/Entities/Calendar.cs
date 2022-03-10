using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9Common;

namespace V9ManagerIVR.Models.Entities
{
    public class Calendar : DefaultEntity
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
        public virtual ICollection<CalendarDayInWeek> CalendarDayInWeeks { get; set; }
        /// <summary>
        /// Nếu là lịch ngày
        /// </summary>
        public virtual List<CalendarDate> CalendarDates { get; set; }
        public Guid CompanyId { get; set; }

        public virtual List<CalendarIVR> CalendarIVRs { get; set; }

    }

    #region Lịch thứ

    /// <summary>
    /// Chi tiết lịch theo thứ
    /// </summary>
    public class CalendarDayInWeek
    {
        public Guid Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public Guid CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual ICollection<CalendarDayInWeekTime> Times { get; set; }
    }
    public class CalendarDayInWeekTime
    {
        public Guid Id { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public Guid CalendarDayInWeekId { get; set; }
        public virtual CalendarDayInWeek CalendarDayInWeek { get; set; }
    }


    #endregion

    #region Lịch ngày

    public class CalendarDate
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public Guid CalendarId { get; set; }
        public virtual Calendar Calendar { get; set; }
    }


    #endregion
}
