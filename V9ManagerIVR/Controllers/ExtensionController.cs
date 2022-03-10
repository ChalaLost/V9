using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Extension;
using V9ManagerIVR.Models.General;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/IVR/[controller]")]
    public class ExtensionController : V9Controller
    {

        private readonly ILogger<ExtensionController> _logger;
        private readonly IExtensionServices _ExtensionServices;

        public ExtensionController(ILogger<ExtensionController> logger, IExtensionServices ExtensionServices)
        {
            _logger = logger;
            _ExtensionServices = ExtensionServices;
        }


        /// <summary>
        /// Danh sách
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllExtension([FromBody] DataTableAjaxPostModel model)
        {
            try
            {
                return Ok(await _ExtensionServices.GetAll(model, Company));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateExtensionModel model)
        {
            try
            {
                if (_ExtensionServices.ValidateUpdate(model, out string message))
                    return BadRequest(message);

                if (!await _ExtensionServices.Update(model, Company, Username, Subdomain))
                    return BadRequest("Cập nhật không thành công, vui lòng liên hệ quản trị");

                //if (!await _ExtensionServices.Update(model, new Guid("99f7bd97-57e7-42f7-828f-ace53d2999e8"), "Binhnc"))
                //    return BadRequest("Cập nhật không thành công, vui lòng liên hệ quản trị");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
