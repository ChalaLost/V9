using Microsoft.AspNetCore.SignalR;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Hubs;

namespace Client.Consumer
{
    public interface IConsumer
    {
        Task Comsume(ConsumeContext<Info> context);
        Task Consume(ConsumeContext<InfoDemo> context);
    }

    public class InfoReceivedConsumer : IConsumer<Info>
    {
        public async Task Consume(ConsumeContext<Info> context)
        {
            await Task.Run(() => { var obj = context.Message; });
        }
        
    }
    public class InfoReceivedConsumerList : IConsumer<InfoDemo>
    {
        private readonly IHubContext<SignalR> _hub;
        public InfoReceivedConsumerList(IHubContext<SignalR> hub){
            _hub = hub;
        }
        public async Task Consume(ConsumeContext<InfoDemo> context)
        {
            await Task.Run(() => { var obj = context.Message.data; });
            List<Info> a =  context.Message.data.OrderByDescending(s =>  s.Id).ToList();
            var obj = context.Message.data;
            await _hub.Clients.All.SendAsync("NewList", obj);
        }
    }
}
