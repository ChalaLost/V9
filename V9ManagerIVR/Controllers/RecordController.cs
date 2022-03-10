using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using V9Common;
using V9ManagerIVR.Models.Company;
using V9ManagerIVR.Models.General;
using V9ManagerIVR.Provider;
using V9ManagerIVR.Services;

namespace V9ManagerIVR.Controllers
{
    [ApiController]
    [Route("api/IVR/[controller]")]
    public class RecordController : V9Controller
    {
        private readonly ILogger<RecordController> _logger;
        private readonly IIVRRecordFileServices _IVRRecordFileServices;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IIntergrationServices _IntergrationServices;
        public RecordController(ILogger<RecordController> logger, IIVRRecordFileServices IVRRecordFileServices,
            IWebHostEnvironment WebHostEnvironment, IIntergrationServices IntergrationServices)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _IVRRecordFileServices = IVRRecordFileServices;
            _logger = logger;
            _IntergrationServices = IntergrationServices;
        }



        /// <summary>
        /// Danh sách file thu âm IVR
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpPost("IVRFile/GetAll")]
        public async Task<IActionResult> GetIVRFilesCompany([FromBody] DataTableAjaxPostModel model)
        {
            try
            {
                model.GuidValue = Company;
                var item = await _IVRRecordFileServices.GetRecordFiles(model);
                return Ok(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Xóa bản ghi theo Id
        /// </summary>
        /// <param name="IVRId"></param>
        /// <returns></returns>
        [HttpDelete("IVRFile/{fileId}")]
        public async Task<IActionResult> DeleteIVRRecordFileCompany(Guid fileId)
        {
            try
            {
                var ssh = new DeleteIVRRecordModel()
                {
                    CompanyCode = Subdomain,
                    SSH_IP = Connect.SSH_IP,
                    SSH_Password = Connect.SSH_Pass,
                    SSH_UserName = Connect.SSH_User
                };

                await _IVRRecordFileServices.DeleteIVRRecordFileCompany(fileId, ssh, Username);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Tạo mới file thu âm IVR
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("CreateIVRFile")]
        public async Task<IActionResult> CreateIVRFileCompany([FromForm] CreateIVRFileCompanyModel model)
        {
            try
            {
                if (model.File.Length == 0) return BadRequest("Vui lòng tải file thu âm");

                #region Save file
                var filename = ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Trim('"');
                filename = filename.RemoveSign4VietnameseString();

                string tempFileName = Username + "-" + Guid.NewGuid().ToString();
                string extensionFile = Path.GetExtension(model.File.FileName);

                var filepath = Path.Combine(_WebHostEnvironment.ContentRootPath, Company.ToString());

                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                string saveFile = $"{filepath}//{tempFileName}{extensionFile}";
                using (FileStream fs = System.IO.File.Create(saveFile))
                {
                    await model.File.CopyToAsync(fs);
                    fs.Flush();
                }
                #endregion

                //Lưu db
                model.CompanyId = Company;
                await _IVRRecordFileServices.CreateIVRRecordFileCompany(model, saveFile, Username);
                _logger.LogError($"Connect Info : {JsonConvert.SerializeObject(Connect)}");
                //Xử lý đẩy sang asterisk
                var _connect = Connect;
                var ssh = new TransitRecordFileIVRCompanyModel()
                {
                    CompanyCode = Subdomain,
                    SSH_IP = _connect.SSH_IP,
                    SSH_Password = _connect.SSH_Pass,
                    SSH_UserName = _connect.SSH_User
                };
                //Chuyển file đến asterisk
                if (!await _IntergrationServices.TransitRecordFileIVRCompany(ssh, saveFile))
                {
                    return BadRequest("Chuyển tiếp file đến asterisk không thành công");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cập nhật file thu âm IVR
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("UpdateIVRFile")]
        public async Task<IActionResult> UpdateIVRFileCompany([FromForm] UpdateIVRFileCompanyModel model)
        {
            try
            {
                if (model.File.Length == 0) return BadRequest("Vui lòng tải file thu âm");

                #region Save file
                var filename = ContentDispositionHeaderValue.Parse(model.File.ContentDisposition).FileName.Trim('"');
                filename = filename.RemoveSign4VietnameseString();

                string tempFileName = Username + "-" + Guid.NewGuid().ToString();
                string extensionFile = Path.GetExtension(model.File.FileName);

                var filepath = _WebHostEnvironment.ContentRootPath + "\\" + Company.ToString();
                if (!Directory.Exists(filepath)) Directory.CreateDirectory(filepath);

                string saveFile = $"{filepath}\\{tempFileName}{extensionFile}";
                using (FileStream fs = System.IO.File.Create(saveFile))
                {
                    await model.File.CopyToAsync(fs);
                    fs.Flush();
                }
                #endregion

                var ssh = new DeleteIVRRecordModel()
                {
                    CompanyCode = Subdomain,
                    SSH_IP = Connect.SSH_IP,
                    SSH_Password = Connect.SSH_Pass,
                    SSH_UserName = Connect.SSH_User
                };
                await _IVRRecordFileServices.UpdateIVRRecordFileCompany(model, saveFile, Username);
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
