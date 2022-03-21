using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;

namespace V9AgentInfo.Hubs
{
    public class SignalR : Hub
    {
        
        public async Task ListInfo(ConsumeContext<InfoDemo> obj)
        {
            await Clients.All.SendAsync("NewList", obj);
        }
    }
}
