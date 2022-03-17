using MassTransit;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;

namespace V9AgentInfo.Hubs
{
    public class SignalR : Hub/*<ISignalR>*/
    {
        /*public async Task ListInfo(Info item)
        {
            await Clients.All.SendAsync("NewList", item);
        }*/
        public async Task ListInfo(ConsumeContext<InfoDemo> obj)
        {
            await Clients.All.SendAsync("NewList", obj);
        }
    }
}
