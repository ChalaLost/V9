using System;
using System.Collections.Generic;

namespace V9ManagerIVR.Models.Entities
{
    /// <summary>
    /// Đầu số của công ty, ví dụ: 19001900
    /// </summary>
    public class Extension : DefaultEntity
    {
        public Guid Id { get; set; }
        /// <summary>
        /// Id bản ghi bên quản trị
        /// </summary>
        public Guid ManagerId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Nhà mạng
        /// </summary>
        public string Network { get; set; }
        /// <summary>
        /// Đầu số
        /// </summary>
        public string Exten { get; set; }
        /// <summary>
        /// Số cuộc gọi đồng thời
        /// </summary>
        public int CurrentCall { get; set; }
        public virtual ICollection<CalendarIVR> CalendarIVRs { get; set; }

    }

    public class CalendarIVR
    {
        public Guid Id { get; set; }
        public Guid? CalendarId { get; set; }
        public Guid? IVRId { get; set; }
        public Guid ExtensionId { get; set; }
        public int? Priority { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual IVR IVR { get; set; }
        public virtual Extension Extension { get; set; }
    }
}
