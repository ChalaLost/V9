using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities.AgentInfo;
using V9MAgentInfo.Models.Entities;

namespace V9AgentInfo.Models.Entities
{
    public class Info 
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Department { get; set; }
        public string Extension { get; set; }
        public int Contacts { get; set; }
        public int Note { get; set; }
        public int OutboundDuration { get; set; }
        public int Outbound { get; set; }
        public int Inbound { get; set; }
        public int Answer { get; set; }
        public int NoAnser { get; set; }
        public int CallPPX { get; set; }
        public string Module { get; set; }

    }

    public class InfoDemo
    {
        public List<Info> data { get; set; }
    }
    
}
