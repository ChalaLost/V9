using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientDemo.Hubs
{
    public class NotifyHub : Hub//INotifyHubClient
    {
        public async Task NotifyReceived(string content, string detail)
        {
            await Clients.All.SendAsync("Received", content, detail);
        }
    }
}
