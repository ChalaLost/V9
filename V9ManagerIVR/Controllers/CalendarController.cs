using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Calendar;
using V9ManagerIVR.Models.General;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/IVR/[controller]")]
    public class CalendarController : V9Controller
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly ICalendarServices _CalendarServices;

        public CalendarController(ILogger<CalendarController> logger, ICalendarServices CalendarServices)
        {
            _logger = logger;
            _CalendarServices = CalendarServices;
        }

        /// <summary>
        /// Tạo mới lịch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CalendarCreateModel model)
        {
            //var check = await _CalendarServices.ValidateCreate(model, Company);
            //if (!check.Item1) return BadRequest(check.Item2);
            await _CalendarServices.Create(model, Username, Company);
            return Ok();
        }

        /// <summary>
        /// Cập nhật lịch
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] CalendarUpdateModel model)
        {
            var check = await _CalendarServices.ValidateUpdate(model, Company);
            if (!check.Item1) return BadRequest(check.Item2);
            await _CalendarServices.Update(model, Username, Company);
            return Ok();
        }

        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Update(Guid id)
        {
            await _CalendarServices.Delete(id, Company, Username);
            return Ok();
        }

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll([FromBody] DataTableAjaxPostModel model)
        {
            try
            {
                return Ok(await _CalendarServices.GetAll(model, Company));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetIVRFilesCompany(Guid id)
        {
            try
            {
                return Ok(await _CalendarServices.GetById(id, Company));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
