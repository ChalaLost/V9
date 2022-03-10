using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;
using V9AgentInfo.Services;

namespace V9AgentInfo.Controllers
{
    [ApiController]
    [Route("api/Info/[controller]")]
    public class AgentInfoController : Controller
    {
        private readonly IInfoServices _InfoServices;
        private readonly ILogger<AgentInfoController> _logger;
        private readonly IBus _busService;
        private readonly IElasticClient _elasticClient;

        public AgentInfoController(IInfoServices InfoServices, ILogger<AgentInfoController> logger, IBus busService, IElasticClient elasticClient)
        {
            _busService = busService;
            _logger = logger;
            _InfoServices = InfoServices;
            _elasticClient = elasticClient;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateInfoModel model)
        {
            try
            {
                var item = await _InfoServices.Create(model);
                Uri uri = new Uri("rabbitmq://localhost/orderQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(item = true);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex);
            }

        }

        [HttpGet("GetById/{InfoId}")]
        public async Task<IActionResult> GetById(Guid InfoId)
        {
            var item = await _InfoServices.GetById(InfoId);
            Uri uri = new Uri("rabbitmq://localhost/orderQueue1");
            var endPoint = await _busService.GetSendEndpoint(uri);
            await endPoint.Send(item);
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var item = await _InfoServices.GetAll();
                Uri uri = new Uri("rabbitmq://localhost/orderQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(new InfoDemo { data = item });
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
        }
        [HttpPut("UpdateContact/{InfoId}")]
        public async Task<IActionResult> UpdateContact(Guid InfoId, [FromBody] UpdateContactInfoModel model)
        {
            try
            {
                var item = await _InfoServices.Update(InfoId, model);
                /*Uri uri = new Uri("rabbitmq://localhost/orderQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(item);*/
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete( Guid Id)
        {
            try
            {
                var item = await _InfoServices.Delete(Id);
                /*Uri uri = new Uri("rabbitmq://localhost/orderQueue");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(item);*/
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
    }
}
