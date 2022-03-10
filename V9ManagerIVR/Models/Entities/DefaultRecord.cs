using System;
using V9Common;

namespace V9ManagerIVR.Models.Entities
{
    public class DefaultRecord : DefaultEntity
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// Đường dẫn thực tế đang lưu trên server
        /// </summary>
        public string RealFileName { get; set; }

        public string Content { get; set; }
        public CreateTypeRecord CreateType { get; set; }
    }
}
