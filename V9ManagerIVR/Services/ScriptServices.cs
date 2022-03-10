using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9Common;
using V9ManagerIVR.Models.CRM;
using V9ManagerIVR.Models.Entities;

namespace V9ManagerIVR.Services
{
    public interface IScriptServices
    {
        /// <summary>
        /// Tạo mới kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<Guid> CreateScript(IVR model, string username);
        /// <summary>
        /// Lấy danh sách kịch bản công ty
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<List<IVR>> GetScriptByCompany(Guid companyId);
        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="company"></param>
        /// <param name="ivrId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task DeleteScript(Guid company, Guid ivrId, string username);
        /// <summary>
        /// Cập nhật kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task UpdateScript(IVR model, Guid company, string username);
        /// <summary>
        /// Lấy danh sách IVR cùng loại
        /// </summary>
        /// <param name="company"></param>
        /// <param name="type"></param>
        /// <param name="currentIVRId"></param>
        /// <returns></returns>
        Task<List<IVR>> GetScriptByAction(Guid company, List<ActionType> type, Guid? currentIVRId);

    }
    public class ScriptServices : IScriptServices
    {
        private readonly V9Context _Context;
        private readonly ServicesMapModel _ServicesMap;
        private readonly ILogger<ScriptServices> _logger;
        public ScriptServices(V9Context Context, IOptions<ServicesMapModel> ServicesMap, ILogger<ScriptServices> logger)
        {
            _Context = Context;
            _ServicesMap = ServicesMap.Value;
            _logger = logger;
        }

        /// <summary>
        /// Tạo mới kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<Guid> CreateScript(IVR model, string username)
        {
            model.CreatedBy = username;
            model.CreatedDate = DateTime.Now;
            model.IsDeleted = false;
            model.Id = Guid.NewGuid();
            await _Context.IVRs.AddAsync(model);

            await _Context.SaveChangesAsync();
            return model.Id;
        }

        /// <summary>
        /// Xóa kịch bản
        /// </summary>
        /// <param name="company"></param>
        /// <param name="ivrId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task DeleteScript(Guid company, Guid ivrId, string username)
        {
            var item = await _Context.IVRs.Where(x => x.CompanyId == company && x.Id == ivrId).FirstOrDefaultAsync();
            if (item != null)
            {
                item.IsDeleted = false;
                item.UpdatedBy = username;
                item.UpdatedDate = DateTime.Now;
                await _Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Cập nhật kịch bản
        /// </summary>
        /// <param name="model"></param>
        /// <param name="company"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task UpdateScript(IVR model, Guid company, string username)
        {
            var item = await _Context.IVRs.Where(x => x.CompanyId == company && x.Id == model.Id).FirstOrDefaultAsync();
            if (item != null)
            {
                if (item.Action != null)
                {
                    if (item.Action.Childrens != null) item.Action.Childrens.Clear();
                    _Context.Actions.Remove(item.Action);
                    await _Context.SaveChangesAsync();
                }
                else
                {
                    item.Action = new Models.Entities.Action();
                }
                item.Name = model.Name;
                item.Action = model.Action;

                item.IsDeleted = false;
                item.UpdatedBy = username;
                item.UpdatedDate = DateTime.Now;
                await _Context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lấy danh sách kịch bản công ty
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public async Task<List<IVR>> GetScriptByCompany(Guid companyId)
        {
            return await _Context.IVRs.Where(x => x.CompanyId == companyId).ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách IVR cùng loại
        /// </summary>
        /// <param name="company"></param>
        /// <param name="type"></param>
        /// <param name="currentIVRId"></param>
        /// <returns></returns>
        public async Task<List<IVR>> GetScriptByAction(Guid company, List<ActionType> type, Guid? currentIVRId)
        {
            var query = _Context.IVRs.Where(x => type.Contains(x.Action.ActionType) && x.CompanyId == company);
            if (currentIVRId.HasValue)
                query = query.Where(x => x.Id != currentIVRId.Value);
            return await query.ToListAsync();
        }

    }
}
