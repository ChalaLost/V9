using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using V9Common;

namespace V9ManagerIVR.Models.CRM
{
    public class CreateIVRModel
    {
        [Required]
        public Guid Company { get; set; }
        public string Exten { get; set; }
        [Required]
        public V9_IVRLevel Level { get; set; }
        [Required]
        public string Name { get; set; }
        public int Priority { get; set; }
        public Guid? ParentId { get; set; }
        [Required]
        public bool IsActive { get; set; }
        public CreateIVRActionModel Action { get; set; }
        public CreateIVRScheduleModel Schedule { get; set; }
        [Required]
        public string UserName { get; set; }
    }

    public class CreateIVRActionModel
    {
        public Guid Id { get; set; }
        public V9_IVRAction Code { get; set; }
        /// <summary>
        /// Phím bấm
        /// </summary>
        public string KeyPress { get; set; }

        #region 1. Thông báo

        /// <summary>
        /// Đường dẫn file thông báo
        /// </summary>
        public string A1_RecordingFile { get; set; }
        /// <summary>
        /// Số lần chạy file thông báo
        /// </summary>
        public int? A1_PlayTimes { get; set; }
        /// <summary>
        /// Thời gian chờ giữa các lần chạy file thông báo, đơn vị tính = giây
        /// Chờ bấm phím
        /// </summary>
        public int? A1_PlayWaittingTime { get; set; }
        /// <summary>
        /// Có chạy nhạc chờ giữa các lần chạy file thông báo hay không
        /// </summary>
        public bool? A1_IsPlayMusicWaitting { get; set; }
        /// <summary>
        /// Đường dẫn nhạc chờ giữa các lần chạy file thông báo
        /// Nếu A1_IsPlayMusicWaitting = false đường dẫn không được tính
        /// </summary>
        public string A1_MusicWaittingFile { get; set; }

        public Guid? ParentId { get; set; }
        /// <summary>
        /// Phím bấm
        /// </summary>
        public List<CreateIVRActionModel> Childrens { get; set; }

        #endregion

        #region 2. Chuyển tiếp
        /// <summary>
        /// Sử dụng chuyển vào queue, hoặc số nội bộ
        /// </summary>
        public bool? A2_IsQueue { get; set; }
        /// <summary>
        /// Tên queue hoặc số nội bộ được chuyển đến
        /// </summary>
        public string A2_TransName { get; set; }
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A2_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A2_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A2_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A2_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A2_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A2_MusicByeFile { get; set; }


        #endregion

        #region 3. Chuyển hướng cuộc gọi
        /// <summary>
        /// Chuyển hướng ra ngoài, danh sách độ ưu tiên với số điện thoại
        /// </summary>
        public List<CreateA3_PhonePriorityModel> A3_PhonePriority { get; set; }
        /// <summary>
        /// Trunk gọi ra
        /// </summary>
        public string A3_Trunk { get; set; }
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A3_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A3_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A3_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A3_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A3_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A3_MusicByeFile { get; set; }

        #endregion

        #region 4. Voicemail
        /// <summary>
        /// Tên hộp thư
        /// </summary>
        public string A4_BoxName { get; set; }
        /// <summary>
        /// File thông báo vào voicemail
        /// </summary>
        public string A4_Music { get; set; }
        #endregion

        #region 5. Định tuyến khách hàng
        public string A5_API_URL { get; set; }
        public V9_APIMethod A5_APIMethod { get; set; }
        public V9_APIAuthenType A5_APIAuthen { get; set; }

        public CreateA5_BasicAuthenModel A5_BasicAuthen { get; set; }
        /// <summary>
        /// Chuỗi JWT
        /// </summary>
        public string A5_APIBearerToken { get; set; }

        public List<CreateA5_APIHeaderModel> A5_APIHeaders { get; set; }

        ///////////////////// - Cấu hình trường hợp không xác định được khách hàng

        /// <summary>
        /// Mặc định trans sang queue, không sẽ sang agent
        /// </summary>
        public bool? A5_IsDefautTransQueue { get; set; }
        /// <summary>
        /// A5_IsDefautTransQueue = false thì là exten, ngược lại là queue
        /// </summary>
        public string A5_DefaultTransName { get; set; }


        ///////////////////// - Cấu hình music
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A5_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A5_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A5_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A5_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A5_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A5_MusicByeFile { get; set; }

        #endregion

        #region 6. Chuyển hướng tương tác
        /// <summary>
        /// Đường dẫn file thông báo
        /// </summary>
        public string A6_RecordingFile { get; set; }
        /// <summary>
        /// Số lần chạy file thông báo
        /// </summary>
        public int? A6_PlayTimes { get; set; }
        /// <summary>
        /// Thời gian chờ giữa các lần chạy file thông báo, đơn vị tính = giây
        /// </summary>
        public int? A6_PlayWaittingTime { get; set; }
        /// <summary>
        /// Có chạy nhạc chờ giữa các lần chạy file thông báo hay không
        /// </summary>
        public bool? A6_IsPlayMusicWaitting { get; set; }
        /// <summary>
        /// Đường dẫn nhạc chờ giữa các lần chạy file thông báo
        /// Nếu A1_IsPlayMusicWaitting = false đường dẫn không được tính
        /// </summary>
        public string A6_MusicWaittingFile { get; set; }
        #endregion

        #region 7. Quay lại menu trước

        #endregion

        #region 8. Bấm sai phím
        /// <summary>
        /// Có cấu hình sai phím,
        /// Khi tạo cây IVR nếu chọn action này thì cần lưu lại để phân biệt
        /// </summary>
        public bool? A8_HasConfig { get; set; }
        /// <summary>
        /// Chạy file thông báo khi nhập sai phím
        /// </summary>
        public string A8_WarningFile { get; set; }
        /// <summary>
        /// Số lần cảnh báo
        /// </summary>
        public int? A8_WarningTimes { get; set; }
        /// <summary>
        /// IVR được chọn làm việc đi khi chọn sai
        /// </summary>
        public Guid? A8_AsNext { get; set; }
        #endregion

        #region 9. Ngắt kết nối
        /// <summary>
        /// File thông báo kết thúc cuộc gọi
        /// </summary>
        public string A9_NotificationFile { get; set; }

        #endregion

        /// <summary>
        /// Ghi chú, mô tả
        /// </summary>
        public string Note { get; set; }

        public CreateForwardActionModel NextAction { get; set; }
    }

    public class CreateForwardActionModel
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Loại hành động
        /// </summary>
        public V9_IVRAction Code { get; set; }
        /// <summary>
        /// Phím bấm
        /// </summary>
        public string KeyPress { get; set; }

        #region 1. Thông báo

        /// <summary>
        /// Đường dẫn file thông báo
        /// </summary>
        public string A1_RecordingFile { get; set; }
        /// <summary>
        /// Số lần chạy file thông báo
        /// </summary>
        public int? A1_PlayTimes { get; set; }
        /// <summary>
        /// Thời gian chờ giữa các lần chạy file thông báo, đơn vị tính = giây
        /// Chờ bấm phím
        /// </summary>
        public int? A1_PlayWaittingTime { get; set; }
        /// <summary>
        /// Có chạy nhạc chờ giữa các lần chạy file thông báo hay không
        /// </summary>
        public bool? A1_IsPlayMusicWaitting { get; set; }
        /// <summary>
        /// Đường dẫn nhạc chờ giữa các lần chạy file thông báo
        /// Nếu A1_IsPlayMusicWaitting = false đường dẫn không được tính
        /// </summary>
        public string A1_MusicWaittingFile { get; set; }
        /// <summary>
        /// Phím bấm
        /// </summary>
        public List<CreateIVRActionModel> A1_PressKeyActions { get; set; }

        #endregion

        #region 2. Chuyển tiếp
        /// <summary>
        /// Sử dụng chuyển vào queue, hoặc số nội bộ
        /// </summary>
        public bool? A2_IsQueue { get; set; }
        /// <summary>
        /// Tên queue hoặc số nội bộ được chuyển đến
        /// </summary>
        public string A2_TransName { get; set; }
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A2_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A2_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A2_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A2_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A2_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A2_MusicByeFile { get; set; }


        #endregion

        #region 3. Chuyển hướng cuộc gọi
        /// <summary>
        /// Chuyển hướng ra ngoài
        /// </summary>
        public List<CreateA3_PhonePriorityModel> A3_PhonePriority { get; set; }
        /// <summary>
        /// Trunk gọi ra
        /// </summary>
        public string A3_Trunk { get; set; }
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A3_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A3_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A3_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A3_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A3_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A3_MusicByeFile { get; set; }

        #endregion

        #region 4. Voicemail
        /// <summary>
        /// Tên hộp thư
        /// </summary>
        public string A4_BoxName { get; set; }
        /// <summary>
        /// File thông báo vào voicemail
        /// </summary>
        public string A4_Music { get; set; }
        #endregion

        #region 5. Định tuyến khách hàng
        public string A5_API_URL { get; set; }
        public V9_APIMethod A5_APIMethod { get; set; }
        public V9_APIAuthenType A5_APIAuthen { get; set; }
        public virtual CreateA5_BasicAuthenModel A5_BasicAuthen { get; set; }
        /// <summary>
        /// Chuỗi JWT
        /// </summary>
        public string A5_APIBearerToken { get; set; }

        public virtual List<CreateA5_APIHeaderModel> A5_APIHeaders { get; set; }

        ///////////////////// - Cấu hình trường hợp không xác định được khách hàng

        /// <summary>
        /// Mặc định trans sang queue, không sẽ sang agent
        /// </summary>
        public bool? A5_IsDefautTransQueue { get; set; }
        /// <summary>
        /// A5_IsDefautTransQueue = false thì là exten, ngược lại là queue
        /// </summary>
        public string A5_DefaultTransName { get; set; }


        ///////////////////// - Cấu hình music
        /// <summary>
        /// Tên file thông báo
        /// </summary>
        public string A5_MusicNotifyFile { get; set; }
        /// <summary>
        /// Tên file nhạc chờ
        /// </summary>
        public string A5_MusicWaitFile { get; set; }
        /// <summary>
        /// Báo bận sau thời gian nghe file nhạc chờ, giây
        /// </summary>
        public int? A5_MusicWaittingTime { get; set; }
        /// <summary>
        /// Tên file báo bận, sau khi nghe nhạc chờ
        /// </summary>
        public string A5_MusicBusyFile { get; set; }
        /// <summary>
        /// Số lần phát lại nhạc chờ
        /// </summary>
        public int? A5_MusicBusyTimes { get; set; }
        /// <summary>
        /// Tên file tạm biệt sau khi kết thúc
        /// </summary>
        public string A5_MusicByeFile { get; set; }

        #endregion

        #region 6. Chuyển hướng tương tác
        /// <summary>
        /// Đường dẫn file thông báo
        /// </summary>
        public string A6_RecordingFile { get; set; }
        /// <summary>
        /// Số lần chạy file thông báo
        /// </summary>
        public int? A6_PlayTimes { get; set; }
        /// <summary>
        /// Thời gian chờ giữa các lần chạy file thông báo, đơn vị tính = giây
        /// </summary>
        public int? A6_PlayWaittingTime { get; set; }
        /// <summary>
        /// Có chạy nhạc chờ giữa các lần chạy file thông báo hay không
        /// </summary>
        public bool? A6_IsPlayMusicWaitting { get; set; }
        /// <summary>
        /// Đường dẫn nhạc chờ giữa các lần chạy file thông báo
        /// Nếu A1_IsPlayMusicWaitting = false đường dẫn không được tính
        /// </summary>
        public string A6_MusicWaittingFile { get; set; }
        #endregion

        #region 7. Quay lại menu trước

        #endregion

        #region 8. Bấm sai phím
        /// <summary>
        /// Có cấu hình sai phím,
        /// Khi tạo cây IVR nếu chọn action này thì cần lưu lại để phân biệt
        /// </summary>
        public bool? A8_HasConfig { get; set; }
        /// <summary>
        /// Chạy file thông báo khi nhập sai phím
        /// </summary>
        public string A8_WarningFile { get; set; }
        /// <summary>
        /// Số lần cảnh báo
        /// </summary>
        public int? A8_WarningTimes { get; set; }
        /// <summary>
        /// IVR được chọn làm việc đi khi chọn sai
        /// </summary>
        public Guid? A8_AsNext { get; set; }
        #endregion

        #region 9. Ngắt kết nối
        /// <summary>
        /// File thông báo kết thúc cuộc gọi
        /// </summary>
        public string A9_NotificationFile { get; set; }

        #endregion

        /// <summary>
        /// Ghi chú, mô tả
        /// </summary>
        public string Note { get; set; }

        public virtual CreateForwardActionModel NextAction { get; set; }
    }

    public class CreateIVRScheduleModel
    {
        public V9_ScheduleActionType Type { get; set; }

        #region Trường thông tin cho lịch ngày
        /// <summary>
        /// Lịch ngày
        /// Lịch ngày có chứa nhiều ngày
        /// nếu Type = 1
        /// </summary>
        public List<CreateScheduleDateType> ScheduleDates { get; set; }
        #endregion

        #region Trường thông tin cho lịch thứ
        /// <summary>
        /// Lịch thứ
        /// </summary>
        public CreateScheduleDayOfWeek ScheduleDayOfWeek { get; set; }
        #endregion

    }

    public class CreateScheduleDateType
    {
        public string DateStr { get; set; }
        public DateTime? Date
        {
            get
            {
                if (!string.IsNullOrEmpty(DateStr) &&
                    DateTime.TryParseExact(DateStr, FormatDate.DateTime_103, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime myDate))
                {
                    return myDate;
                }
                return null;
            }
        }
        /// <summary>
        /// Trong lich ngày có chứa lịch
        /// </summary>
        //Trong lịch ngày có chứa danh sách các thời gian trong ngày
        public List<CreateScheduleDateTypeTime> Times { get; set; }
    }

    public class CreateScheduleDateTypeTime
    {
        /// <summary>
        /// Từ thời gian, định dạng HH:mm
        /// </summary>
        public string FromTimeStr { get; set; }
        public TimeSpan? FromTime
        {
            get
            {
                if (!string.IsNullOrEmpty(FromTimeStr) &&
                    DateTime.TryParseExact(FromTimeStr, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime myDate))
                {
                    return myDate.TimeOfDay;
                }
                return null;
            }
        }
        /// <summary>
        /// Đến thời gian, định dạng HH:mm
        /// </summary>
        public string ToTimeStr { get; set; }
        public TimeSpan? ToTime
        {
            get
            {
                if (!string.IsNullOrEmpty(ToTimeStr) &&
                    DateTime.TryParseExact(ToTimeStr, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime myDate))
                {
                    return myDate.TimeOfDay;
                }
                return null;
            }
        }
    }


    public class CreateScheduleDayOfWeek
    {
        /**
         Trong lịch thứ sẽ có nhiều thứ, từ, thứ 2 đến CN
         Và có nhiều tháng được chọn hoặc bỏ chọn
         */
        /// <summary>
        /// Thứ được chọn
        /// </summary>
        public List<CreateDayOfWeek> DayOfWeeks { get; set; }
        /// <summary>
        /// Tháng được chọn
        /// </summary>
        public List<CreateMonth> Months { get; set; }
    }

    public class CreateDayOfWeek
    {
        public DayOfWeek Value { get; set; }
        public List<CreateDayOfWeekTimes> Times { get; set; }
    }


    public class CreateDayOfWeekTimes
    {
        public string FromTimeStr { get; set; }
        public string ToTimeStr { get; set; }
        public TimeSpan? FromTime
        {
            get
            {
                if (!string.IsNullOrEmpty(FromTimeStr) &&
                    DateTime.TryParseExact(FromTimeStr, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime myDate))
                {
                    return myDate.TimeOfDay;
                }
                return null;
            }
        }
        public TimeSpan? ToTime
        {
            get
            {
                if (!string.IsNullOrEmpty(ToTimeStr) &&
                    DateTime.TryParseExact(ToTimeStr, FormatDate.HHmm, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime myDate))
                {
                    return myDate.TimeOfDay;
                }
                return null;
            }
        }
    }


    public class CreateMonth
    {
        public int Value { get; set; }
    }

    public class CreateIVRScheduleTime
    {
        /// <summary>
        /// Chọn thứ -> thời gian 
        /// </summary>
        public string FromTime { get; set; }
        /// <summary>
        /// Chọn thứ -> thời gian 
        /// </summary>
        public string ToTime { get; set; }
        /// <summary>
        /// Các thứ trong tuần, ngăn cách = dấu ,
        /// </summary>
        public System.DayOfWeek Day { get; set; }

    }

    public class CreateIVRScheduleExclude
    {
        public string Date { get; set; }
    }


    public class CreateA3_PhonePriorityModel
    {
        /// <summary>
        /// Thứ tự tăng dần
        /// </summary>
        public int Priority { get; set; }
        public string Phone { get; set; }
    }

    public class CreateA5_APIHeaderModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class CreateA5_BasicAuthenModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }


}
