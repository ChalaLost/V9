
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private readonly V9Context _Context;

        public AgentNotifyController(INotifyServices NotifyServices, ILogger<AgentNotifyController> logger, V9Context Context)
        {
            _logger = logger;
            _NotifyServices = NotifyServices;
            _Context = Context;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateNotifyModel model)
        {
            try
            {
                var item = await _NotifyServices.Create(model);
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
            return Ok(item);
        }
    }
}
