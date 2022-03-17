using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using V9AgentInfo.Models.Entities;
using V9AgentInfo.Models.Entities.AgentInfo;

namespace V9AgentInfo.Services
{
    public interface INotifyServices
    {
        Task<bool> Create(CreateNotifyModel model);
        Task<List<Notify>> GetAll();

    }
    public class Notifyservices : INotifyServices
    {
        private readonly V9Context _Context;
        private readonly ILogger<Notifyservices> _logger;

        public Notifyservices(V9Context Context, ILogger<Notifyservices> logger)
        {
            _logger = logger;
            _Context = Context;

        }
        public async Task<bool> Create(CreateNotifyModel model)
        {
            var guid = Guid.NewGuid();
            var item = new Notify()
            {
                Id = guid,
                Content = model.Content,
                Detail = model.Detail,
            };
            await _Context.Notifys.AddAsync(item);
            await _Context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Notify>> GetAll()
        {
            
            var a = await _Context.Notifys.ToListAsync();
            return a;
        }
    }
}
