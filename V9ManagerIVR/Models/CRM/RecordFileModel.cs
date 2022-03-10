using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9Common;

namespace V9ManagerIVR.Models.CRM
{
    public class ListRecordFileModel : DefaultModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }

        public CreateTypeRecord CreateType { get; set; }
        public string Content { get; set; }
    }
}
