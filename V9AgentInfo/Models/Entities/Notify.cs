using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9AgentInfo.Models.Entities
{
    public class Notify
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Detail { get; set; }
    }
}
