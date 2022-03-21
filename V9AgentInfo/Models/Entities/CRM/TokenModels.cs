using System;
using System.Collections.Generic;

namespace V9AgentInfo.Models.CRM
{
    public class UserTokenDTO
    {
        public string Id { get; set; }
        public string Accesstoken { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Permissions { get; set; }
    }
}
