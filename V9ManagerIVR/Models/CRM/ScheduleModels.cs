using System;
using System.Collections.Generic;
using V9Common;

namespace V9ManagerIVR.Models.CRM
{
    public class ScheduleModel
    {

        public V9_ScheduleActionType Type { get; set; }

        #region Trường thông tin cho lịch ngày
        /// <summary>
        /// Lịch ngày có chứa nhiều ngày
        /// nếu Type = 1
        /// </summary>
        public List<ScheduleDateTypeModel> ScheduleDates { get; set; }
        #endregion

        #region Trường thông tin cho lịch thứ
        public ScheduleDayOfWeekModel ScheduleDayOfWeek { get; set; }

        #endregion
        /// <summary>
        /// Mã lịch
        /// </summary>
        public Guid IVRId { get; set; }
    }

    #region - Entities lịch ngày
    /// <summary>
    /// Lưu trữ các thời gian trong lịch ngày
    /// Trong lịch ngày có danh sách ngày
    /// Trong ngày có nhiều thời gian 
    /// eg: 8h00 - 12h00, 12h00 - 14h00, 14h00-17h00
    /// </summary>
    public class ScheduleDateTypeModel
    {
        public DateTime Date { get; set; }
        /// <summary>
        /// Trong lich ngày có chứa lịch
        /// </summary>
        //Trong lịch ngày có chứa danh sách các thời gian trong ngày
        public List<ScheduleDateTypeTimeModel> Times { get; set; }
    }

    public class ScheduleDateTypeTimeModel
    {
        /// <summary>
        /// Từ thời gian
        /// </summary>
        public TimeSpan FromTime { get; set; }
        /// <summary>
        /// Đến thời gian
        /// </summary>
        public TimeSpan ToTime { get; set; }
    }


    #endregion

    #region - Entities lịch thứ
    /**
     Lịch thứ chỉ được lưu vào trong 1 đối tượng
     Các quan hệ trong nó lại tiếp tục đi
     */
    public class ScheduleDayOfWeekModel
    {
        /// <summary>
        /// Thứ được chọn
        /// </summary>
        public List<DayOfWeekModel> DayOfWeeks { get; set; }
        /// <summary>
        /// Tháng được chọn
        /// </summary>
        public List<MonthModel> Months { get; set; }

    }

    /// <summary>
    /// Thứ được chọn
    /// </summary>
    public class DayOfWeekModel
    {
        public System.DayOfWeek Value { get; set; }
        public List<DayOfWeekTimesModel> Times { get; set; }
    }

    /// <summary>
    /// Thời gian của các thứ
    /// </summary>
    public class DayOfWeekTimesModel
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }

    /// <summary>
    /// Tháng được chọn
    /// </summary>
    public class MonthModel
    {
        public int Value { get; set; }
    }

    #endregion

    public class IVRToDayModel
    {
        public Guid IVRId { get; set; }
        public int Priority { get; set; }
        public List<IVRToDayTimeModel> Times { get; set; }
    }

    public class IVRToDayResultModel
    {
        public Guid IVRId { get; set; }
        public IVRToDayModel Data { get; set; }
    }

    public class IVRToDayTimeModel
    {
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
    }
}
