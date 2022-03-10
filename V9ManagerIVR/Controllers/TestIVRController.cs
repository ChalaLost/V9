using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Entities;
using V9ManagerIVR.Models.Extension;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestIVRController : ControllerBase
    {

        private readonly ILogger<TestIVRController> _logger;
        private readonly IElasticServices _ElasticServices;
        private readonly V9Context _Context;

        public TestIVRController(ILogger<TestIVRController> logger, IElasticServices ElasticServices, V9Context context)
        {
            _Context = context;
            _logger = logger;
            _ElasticServices = ElasticServices;
        }

        /// <summary>
        /// Lấy tất cả lịch IVR chạy trong ngày
        /// </summary>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create(ExtensionElasticModel model)
        {
            try
            {
                await _ElasticServices.CreateExtensionAsync(model);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Lỗi hệ thống");
            }
        }

        /// <summary>
        /// Kiểm tra extension tồn tại
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("CheckExitstById/{id}")]
        public async Task<IActionResult> CheckExitstById(Guid id)
        {
            try
            {
                return Ok(await _ElasticServices.CheckExitstById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Lỗi hệ thống");
            }
        }


        /// <summary>
        /// bulk
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("BulkUpsertExtension")]
        public async Task<IActionResult> BulkUpsertExtension([FromBody] List<ExtensionElasticModel> model)
        {
            try
            {
                return Ok(await _ElasticServices.BulkUpsertListExtension(model));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Lỗi hệ thống");
            }
        }

        /// <summary>
        /// Lấy tất cả lịch IVR chạy trong ngày
        /// </summary>
        /// <returns></returns>
        [HttpGet("Count")]
        public async Task<IActionResult> Count()
        {
            try
            {
                await _Context.Database.EnsureCreatedAsync();
                return Ok(await _ElasticServices.ExtensionCountAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest("Lỗi hệ thống");
            }
        }
    }
}
