using System;

namespace V9ManagerIVR.Models.Entities.V9Role
{
    public class Queue : DefaultEntity
    {
        public Guid Id { get; set; }
        public string QueueName { get; set; }
        public string QueueCode { get; set; }
        public string Strategy { get; set; }
        public string ExtenOutbound { get; set; }
        public string SubDomain { get; set; }
        public Guid CompanyId { get; set; }
        public bool DefaultOutboundCompany { get; set; }
    }
}
