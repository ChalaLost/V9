using System;
using System.Collections.Generic;
using V9Common;

namespace V9ManagerIVR.Models.Entities
{
    public class IIIIVR
    {
        public Guid Id { get; set; }
        public Guid Company { get; set; }
        /// <summary>
        /// Phân biệt lv để trả ra các màn hình tương ứng
        /// lv0 = 1: tên mặc định = V9
        /// lv1 = 2: là tên và cho gắn đầu số
        /// lv2 = 3: là tên và cho gắn lịch và hành động, không cho chọn phím bấm
        /// lvn = 0: các hành động con trong lv2
        /// </summary>
        public V9_IVRLevel Level { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        /// <summary>
        /// Độ ưu tiên sử dụng
        /// TH trong cùng thời gian có độ ưu tiên thấp nhất, tính từ 1,
        /// Không được để nhiều độ ưu tiên = nhau, cần validate kỹ đoạn này
        /// </summary>
        public int Priority { get; set; }

        public Guid? ParentId { get; set; }
        public virtual IVR Parent { get; set; }
        public virtual ICollection<IVR> Childrens { get; set; }

        public Guid? ActionId { get; set; }
        public virtual Action Action { get; set; }

        public Guid? ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<IVRExten> Extens { get; set; }
    }

    /// <summary>
    /// Hành động có phím bấm
    /// </summary>
    public class IIIAction
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

        public Guid? ParentId { get; set; }
        public virtual Action Parent { get; set; }
        /// <summary>
        /// Phím bấm
        /// </summary>
        public virtual List<Action> Childrens { get; set; }

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
        public virtual List<A3_PhonePriority> A3_PhonePriority { get; set; }
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

        public Guid? A5_BasicAuthenId { get; set; }
        public virtual A5_BasicAuthen A5_BasicAuthen { get; set; }
        /// <summary>
        /// Chuỗi JWT
        /// </summary>
        public string A5_APIBearerToken { get; set; }

        public virtual List<A5_APIHeaders> A5_APIHeaders { get; set; }

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

        public Guid? IVRId { get; set; }
        public virtual IVR IVR { get; set; }

        /// <summary>
        /// Cho mối quan hệ 1 - 1
        /// </summary>
        public Guid? ForwardActionId { get; set; }
        /// <summary>
        /// Cho mối quan hệ 1 - 1
        /// </summary>
        public virtual ForwardAction NextAction { get; set; }
    }

    /// <summary>
    /// Hành động tiếp theo
    /// </summary>
    public class ForwardAction
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
        public virtual List<Action> A1_KeyPressActions { get; set; }

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
        public virtual List<A3_PhonePriority> A3_PhonePriority { get; set; }
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

        public Guid? A5_BasicAuthenId { get; set; }
        public virtual A5_BasicAuthen A5_BasicAuthen { get; set; }
        /// <summary>
        /// Chuỗi JWT
        /// </summary>
        public string A5_APIBearerToken { get; set; }

        public virtual List<A5_APIHeaders> A5_APIHeaders { get; set; }

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

        public Guid? ForwardActionId { get; set; }
        public virtual ForwardAction NextAction { get; set; }

        public Guid? BackActionId { get; set; }
        public virtual ForwardAction BackAction { get; set; }
    }

    /// <summary>
    /// Lịch sử dụng IVR
    /// </summary>
    public class Schedule
    {
        public Guid Id { get; set; }
        public V9_ScheduleActionType Type { get; set; }

        #region Trường thông tin cho lịch ngày
        /// <summary>
        /// Lịch ngày
        /// Lịch ngày có chứa nhiều ngày
        /// nếu Type = 1
        /// </summary>
        public virtual ICollection<ScheduleDateType> ScheduleDates { get; set; }
        #endregion

        #region Trường thông tin cho lịch thứ
        public Guid? ScheduleDayOfWeekId { get; set; }
        /// <summary>
        /// Lịch thứ
        /// </summary>
        public virtual ScheduleDayOfWeek ScheduleDayOfWeek { get; set; }

        #endregion

        public Guid? IVRId { get; set; }
        public virtual IVR IVR { get; set; }
    }

    #region - Entities lịch ngày
    /// <summary>
    /// Lưu trữ các thời gian trong lịch ngày
    /// Trong lịch ngày có danh sách ngày
    /// Trong ngày có nhiều thời gian 
    /// eg: 8h00 - 12h00, 12h00 - 14h00, 14h00-17h00
    /// </summary>
    public class ScheduleDateType
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        public Guid ScheduleId { get; set; }
        /// <summary>
        /// Trong lich ngày có chứa lịch
        /// </summary>
        public virtual Schedule Schedule { get; set; }
        //Trong lịch ngày có chứa danh sách các thời gian trong ngày
        public virtual ICollection<ScheduleDateTypeTime> Times { get; set; }
    }

    public class ScheduleDateTypeTime
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Từ thời gian
        /// </summary>
        public TimeSpan FromTime { get; set; }
        /// <summary>
        /// Đến thời gian
        /// </summary>
        public TimeSpan ToTime { get; set; }

        public Guid ScheduleDateTypeId { get; set; }
        /// <summary>
        /// Trong thời gian trong ngày có chứa lịch ngày
        /// </summary>
        public virtual ScheduleDateType ScheduleDateType { get; set; }
    }


    #endregion

    #region - Entities lịch thứ
    /**
     Lịch thứ chỉ được lưu vào trong 1 đối tượng
     Các quan hệ trong nó lại tiếp tục đi
     */
    public class ScheduleDayOfWeek
    {
        /**
         Trong lịch thứ sẽ có nhiều thứ, từ, thứ 2 đến CN
         Và có nhiều tháng được chọn hoặc bỏ chọn
         */
        public Guid Id { get; set; }
        /// <summary>
        /// Thứ được chọn
        /// </summary>
        public virtual ICollection<DayOfWeeks> DayOfWeeks { get; set; }
        /// <summary>
        /// Tháng được chọn
        /// </summary>
        public virtual ICollection<Month> Months { get; set; }

        public Guid ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

    }

    /// <summary>
    /// Thứ được chọn
    /// </summary>
    public class DayOfWeeks
    {
        public Guid Id { get; set; }
        public DayOfWeek Value { get; set; }
        public virtual ICollection<DayOfWeekTimes> Times { get; set; }

        public Guid ScheduleDayOfWeekId { get; set; }
        public virtual ScheduleDayOfWeek ScheduleDayOfWeek { get; set; }

    }

    public class DayOfWeekTimes
    {
        public Guid Id { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }

        public Guid DayOfWeekId { get; set; }
        public virtual DayOfWeeks DayOfWeek { get; set; }


    }

    /// <summary>
    /// Tháng được chọn
    /// </summary>
    public class Month
    {
        public Guid Id { get; set; }
        public int Value { get; set; }


        public Guid ScheduleDayOfWeekId { get; set; }
        public virtual ScheduleDayOfWeek ScheduleDayOfWeek { get; set; }
    }

    #endregion

    /// <summary>
    /// Danh sách số điện thoại và độ ưu tiên
    /// </summary>
    public class A3_PhonePriority
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Thứ tự tăng dần
        /// </summary>
        public int Priority { get; set; }
        public string Phone { get; set; }

        public Guid? ForwardActionId { get; set; }
        public virtual ForwardAction ForwardAction { get; set; }

        public Guid? ActionId { get; set; }
        public virtual Action Action { get; set; }

    }

    /// <summary>
    /// Các header cho hành động gọi API
    /// </summary>
    public class A5_APIHeaders
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public Guid? ActionId { get; set; }
        public virtual Action Action { get; set; }

        public Guid? ForwardActionId { get; set; }
        public virtual ForwardAction ForwardAction { get; set; }

    }

    /// <summary>
    /// Nếu chọn loại authen là basic thì đây chính là nó
    /// </summary>
    public class A5_BasicAuthen
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public Guid? ActionId { get; set; }
        public virtual Action Action { get; set; }

        public Guid? ForwardActionId { get; set; }
        public virtual ForwardAction ForwardAction { get; set; }
    }

    public class IVRExten
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Đầu số
        /// </summary>
        public string Exten { get; set; }
        /// <summary>
        /// Nhà mạng
        /// </summary>
        public string Provider { get; set; }
        public Guid IVRId { get; set; }

        public virtual IVR IVR { get; set; }
    }
}
