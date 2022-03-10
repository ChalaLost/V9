using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9ManagerIVR.Models.CRM
{
    public class ConnectionDatabaseModel
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    

    public class GetDatabaseInfoByCompanyResponseModel
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public ConnectionDatabaseModel Info { get; set; }
    }
}
