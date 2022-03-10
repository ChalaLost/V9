using System;

namespace V9ManagerIVR.Models.Entities.V9Role
{
    public class Extension : DefaultEntity
    {
        public Guid Id { get; set; }
        public int ExtensionNumber { get; set; }
        public string Password { get; set; }
        public string Callerid_tag { get; set; }
        public string SubDomain { get; set; }
        public Guid CompanyId { get; set; }
    }
}
