using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Hubs;
using V9AgentInfo.Models;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;
using V9AgentInfo.Services;

namespace V9AgentInfo.Controllers
{
    [ApiController]
    [Route("api/Notify/[controller]")]
    public class AgentNotifyController : Controller
    {
        private readonly INotifyServices _NotifyServices;
        private readonly ILogger<AgentNotifyController> _logger;
        private readonly IHubContext<NotificationHub, INotifyHubClient> _hubContext;
        private readonly V9Context _Context;

        public AgentNotifyController(INotifyServices NotifyServices, ILogger<AgentNotifyController> logger, IHubContext<NotificationHub, INotifyHubClient> hubContext, V9Context Context)
        {
            _logger = logger;
            _NotifyServices = NotifyServices;
            _hubContext = hubContext;
            _Context = Context;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateNotifyModel model)
        {
            try
            {
                var item = await _NotifyServices.Create(model);
                await _hubContext.Clients.All.CreateBroadcastNotify(model);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }

        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var item = await _NotifyServices.GetAll();
            var notify = await _Context.Notifys.ToListAsync();
            await _hubContext.Clients.All.BroadcastNotify(notify);
            return Ok(item);
        }
    }
}
