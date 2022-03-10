using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9ManagerIVR.Models.General
{
    public class DataTableAjaxPostModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public Guid? GuidValue { get; set; }
    }
}
