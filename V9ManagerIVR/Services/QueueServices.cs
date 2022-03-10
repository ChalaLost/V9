using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9ManagerIVR.Models.Entities.V9Role;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Services
{
    public interface IQueueServices
    {
        /// <summary>
        /// Lấy ra select box
        /// </summary>
        /// <param name="company"></param>
        /// <param name="selectedQueue"></param>
        /// <param name="selectedExten"></param>
        /// <returns></returns>
        Task<List<Select2Model>> GetSelectQueueCode(Guid company, string selectedQueue = null, int? selectedExten = null);
    }
    public class QueueServices : IQueueServices
    {
        private readonly ILogger<QueueServices> _logger;
        private readonly RoleContext _RoleContext;
        public QueueServices(ILogger<QueueServices> logger, RoleContext RoleContext)
        {
            _RoleContext = RoleContext;
            _logger = logger;
        }

        /// <summary>
        /// Lấy ra select box
        /// </summary>
        /// <param name="company"></param>
        /// <param name="selectedQueue"></param>
        /// <param name="selectedExten"></param>
        /// <returns></returns>
        public async Task<List<Select2Model>> GetSelectQueueCode(Guid company, string selectedQueue = null, int? selectedExten = null)
        {
            var queue = await _RoleContext.Queues.Where(x => x.CompanyId == company).Select(s => new Select2Model
            {
                Id = s.QueueCode,
                Text = s.QueueName,
                Selected = s.QueueCode == selectedQueue,
                Type = "Queue"
            }).ToListAsync();

            var extension = await _RoleContext.Extensions.Where(x => x.CompanyId == company).Select(s => new Select2Model
            {
                Id = s.ExtensionNumber.ToString(),
                Text = s.ExtensionNumber.ToString(),
                Selected = selectedExten.HasValue && s.ExtensionNumber == selectedExten.Value,
                Type = "Exten"
            }).ToListAsync();

            if (queue != null) queue.AddRange(extension);
            return queue;
        }
    }
}
