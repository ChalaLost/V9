using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;

namespace V9AgentInfo.Services
{
    public interface IInfoServices
    {
        Task<bool> Create( CreateInfoModel model);
        Task<Info> GetById(Guid InfoId);
        Task<bool> Update(Guid InfoId, UpdateContactInfoModel model);
        Task<bool> Delete(Guid Ids);
        Task<List<Info>> GetAll();
    }

    public class Infoservices : IInfoServices
    {
        private readonly V9Context _Context;
        private readonly ILogger<Infoservices> _logger;

        public Infoservices(V9Context Context, ILogger<Infoservices> logger)
        {
            _logger = logger;
            _Context = Context;

        }
        public async Task<bool> Create(CreateInfoModel model)
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
            return true;
        }
        public async Task<List<Info>> GetAll()
        {
            return await _Context.Infos.ToListAsync();
        }

        public async Task<Info> GetById(Guid InfoId)
        {
            return await _Context.Infos.Where(x => x.Id == InfoId).FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Guid InfoId, UpdateContactInfoModel model)
        {
            try
            {

                var find = await _Context.Infos.Where(x => x.Id == InfoId).FirstOrDefaultAsync();
                find.Contacts = model.Contacts + 1;
                await _Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        public async Task<bool> Delete(Guid Ids)
        {
            try
            {
                var item = await _Context.Infos.FindAsync(Ids);
                if (item != null)
                {
                    _Context.Infos.Remove(item);
                    await _Context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
    }
}
