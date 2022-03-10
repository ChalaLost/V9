using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;

namespace V9AgentInfo.Models
{
    public interface INotifyHubClient
    {
        Task CreateBroadcastNotify(CreateNotifyModel createnotify);
        Task BroadcastNotify(List<Notify> notify);
    }
}
