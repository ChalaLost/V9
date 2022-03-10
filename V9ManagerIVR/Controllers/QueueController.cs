using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/IVR/[controller]")]
    public class QueueController : V9Controller
    {
        private readonly ILogger<QueueController> _logger;
        private readonly IQueueServices _QueueServices;

        public QueueController(ILogger<QueueController> logger, IQueueServices QueueServices)
        {
            _QueueServices = QueueServices;
            _logger = logger;
        }

        /// <summary>
        /// Lấy ra select queue sử dụng queuecode
        /// </summary>
        /// <param name="selected"></param>
        /// <returns></returns>
        [HttpGet("GetSelectQueueCode")]
        public async Task<IActionResult> GetSelectQueueCode()
        {
            try
            {
                return Ok(await _QueueServices.GetSelectQueueCode(Company));
                //return Ok(await _QueueServices.GetSelectQueueCode(new Guid("99f7bd97-57e7-42f7-828f-ace53d2999e8")));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }

        }

    }
}
