using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9AgentInfo.Models.Entities.AgentInfo
{
    public class CreateInfoModel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }
        public string Extension { get; set; }
        public string Module { get; set; }
    }

    public class CreateInfoModelDemo
    {
        public CreateInfoModel createInfoModel { get; set; }
    }
}
