using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9AgentInfo.Models.Entities.CRM
{
    public class LoginModel
    {
        public string UserName { get; set; }
    }
    public class UserLogonModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Permissions { get; set; }
        public List<Guid> RoleIds { get; set; }
    }
}
