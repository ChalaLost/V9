using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V9Common
{
    #region ENUM

    /// <summary>
    /// Hình thức tạo file voice cho IVR
    /// </summary>
    public enum CreateTypeRecord : int
    {
        /// <summary>
        /// Mặc định khi tạo công ty
        /// </summary>
        Default = 1,
        /// <summary>
        /// Ghi âm trực tiếp trên hệ thống V9
        /// </summary>
        RecordingDirection = 2,
        /// <summary>
        /// Người dùng upload lên
        /// </summary>
        Upload = 3
    }
    public enum SubActionPermission
    {
        None = 0,
        All = 1,
        Department = 2,
        Group = 3,
        Private = 4
    }

    /// <summary>
    /// Loại action
    /// </summary>
    public enum ActionType : int
    {
        IVR = 1,
        ChuyenCuocGoiToiNhom = 2,
        ChuyenCuocGoiRaDiDong = 3,
        VoiceMail = 4,
        TraLoiTuDong = 5,
        KetThucCuocGoi = 6
    }

    /// <summary>
    /// Phân biệt action con là phím bấm hay không
    /// </summary>
    public enum ActionCode : int
    {
        PhimBam = 1,
        TrucThuoc = 2
    }

    public enum ActionCode2_ActionType
    {
        None = 0,
        /// <summary>
        /// Phát hết thông báo mà khách hàng không bấm phím
        /// Hành động con trong hành động IVR
        /// </summary>
        IVR_Type1 = 1,
        /// <summary>
        /// Nếu khách hàng bấm sai phím vượt tối đa
        /// Hành động con trong hành động IVR
        /// </summary>
        IVR_Type2 = 2,
        /// <summary>
        /// Nếu không gặp được điện thoại viên
        /// ành động con trong hành động: Chuyển cuộc gọi tới nhóm
        /// </summary>
        TG_Type = 3
    }

    public enum V9_APITransferType
    {
        Exten = 1,
        Queue = 2
    }

    public enum V9_APIMethod : int
    {
        POST = 0,
        GET = 1
    }

    public enum V9_APIAuthenType : int
    {
        NoAuthen = 0,
        Basic = 1,
        JWT = 2
    }

    public enum V9_QueueIVR : int
    {
        /// <summary>
        /// Lấy ra
        /// </summary>
        Get = 1,
        /// <summary>
        /// Đưa vào
        /// </summary>
        Set = 2
    }

    public enum V9_IVRLevel : int
    {
        /// <summary>
        /// Thư mục gốc
        /// </summary>
        LV1 = 1,
        /// <summary>
        /// level 2, tên IVR
        /// </summary>
        LV2 = 2,
        /// <summary>
        /// Level 3, bắt đầu cho phép thêm action, lịch
        /// </summary>
        LV3 = 3,
        /// <summary>
        /// level n
        /// </summary>
        LVn = 0
    }
    public enum V9_IVRAction : int
    {
        ThongBao = 1,
        ChuyenTiep = 2,
        ChuyenHuongCuocGoi = 3,
        VoiceMail = 4,
        DinhTuyenKhachHang = 5,
        /// <summary>
        /// Lựa chọn menu có hành động thông báo để chọn
        /// </summary>
        ChuyenHuongTuongTac = 6,
        /// <summary>
        /// Tự động nhận biết để trở về đầu IVR hiện tại
        /// </summary>
        QuayLaiMenu = 7,
        NhapSaiPhim = 8,
        KetThuc = 9
    }
    /// <summary>
    /// Loại lịch
    /// </summary>
    public enum V9_ScheduleActionType
    {
        /// <summary>
        /// Theo thứ
        /// </summary>
        DayOfWeek = 1,
        /// <summary>
        /// Theo ngày
        /// </summary>
        Date = 2
    }

    #endregion


    #region CONST

    public static class EmailTemplateCode
    {
        public const string QuenMatKhauQuanTri = "QuenMatKhauQuanTri";
        public const string HeThong = "b02ed255-108d-4c1f-b831-b9507401cccd";
        public const string QuenMatKhauKhachHang = "QuenMatKhauKhachHang";
        public const string Khac = "ConfigEmail02";
        public const string TaoMoiTaiKhoanCongTy = "TaoMoiTaiKhoanCongTy";
        public const string TaoMoiTaiKhoan = "TaoMoiTaiKhoan";
        public const string KichHoatTaiKhoan = "KichHoatTaiKhoan";

    }

    public static class CategoryConst
    {
        public const string LoaiPhanHoi = "LoaiPhanHoi";
    }

    #endregion

}
