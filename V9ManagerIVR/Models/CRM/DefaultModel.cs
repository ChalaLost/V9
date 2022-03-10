using System;
using V9Common;

namespace V9ManagerIVR.Models.CRM
{
    public class DefaultModel
    {
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get => CreatedDate.ToString(FormatDate.DateTime_ddMMyyyyHHmm); }
        public string CreatedBy { get; set; }
    }
}
