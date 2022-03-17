using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;

namespace RabbitDemo.Consumer
{
    public class InfoReceivedConsumer : IConsumer<Info>
    {
        public async Task Consume(ConsumeContext<Info> context)
        {
            await Task.Run(() => { var obj = context.Message; });
        }
    }
}
