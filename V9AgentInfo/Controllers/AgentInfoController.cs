using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Hubs;
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
        private readonly V9Context _Context;
        private readonly IElasticClient _elasticClient;
        private readonly IHubContext<SignalR> _hub;

        public AgentInfoController(IInfoServices InfoServices, ILogger<AgentInfoController> logger, IBus busService, IElasticClient elasticClient, V9Context Context, IHubContext<SignalR> hub)
        {
            _busService = busService;
            _logger = logger;
            _InfoServices = InfoServices;
            _elasticClient = elasticClient;
            _Context = Context;
            _hub = hub;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateInfoModel model)
        {
            try
            {
                var guidID = Guid.NewGuid();
                var guidComId = Guid.NewGuid();
                var item = new Info()
                {
                    Id = guidID,
                    CompanyId = guidComId,
                    UserName = model.UserName,
                    FullName = model.FullName,
                    Image = model.Image,
                    Department = model.Department,
                    Extension = model.Extension,
                    Module = model.Module,
                };
                await _Context.Infos.AddAsync(item);
                await _Context.SaveChangesAsync();
                Uri uri = new Uri("rabbitmq://localhost/CreateInfo");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(item);
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
            Uri uri = new Uri("rabbitmq://localhost/GetIdInfo");
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
                //topic exchange
                Uri uri = new Uri("rabbitmq://localhost/GetAll_Info");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(new InfoDemo { data = item });
                return Ok(item);
            }
            catch (Exception e)
            {
                return BadRequest(e);
                throw;
            }
            /*var ttl = new Dictionary<string, object>
            {
                {"x-message-ttl", 30000 }
            };
            channel.ExchangeDeclare("demo-topic-exchange", ExchangeType.Topic, arguments: ttl);
            var count = 0;

            while (true)
            {
                var message = new { Name = "Producer", Message = $"Hello! Count: {count}" };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish("demo-topic-exchange", "user.update", null, body);
                count++;
                Thread.Sleep(1000);
            }*/
        }
        [HttpPut("UpdateContact/{InfoId}")]
        public async Task<IActionResult> UpdateContact(Guid InfoId, [FromBody] UpdateContactInfoModel model)
        {
            try
            {
                var find = await _Context.Infos.Where(x => x.Id == InfoId).FirstOrDefaultAsync();
                find.Contacts = model.Contacts + 1;
                await _Context.SaveChangesAsync();
                Uri uri = new Uri("rabbitmq://localhost/UpdateContactInfo");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(find);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Ids)
        {
            try
            {
                var item = await _Context.Infos.FindAsync(Ids);
                if (item != null)
                {
                    _Context.Infos.Remove(item);
                    await _Context.SaveChangesAsync();
                }
                Uri uri = new Uri("rabbitmq://localhost/DeleteInfo");
                var endPoint = await _busService.GetSendEndpoint(uri);
                await endPoint.Send(item);
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
