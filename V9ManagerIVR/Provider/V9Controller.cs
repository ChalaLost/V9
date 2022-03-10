using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using V9Common;
using V9ManagerIVR.Models.General;

namespace V9ManagerIVR.Provider
{

    public class V9Controller : ControllerBase
    {
        protected string Username
        {
            get
            {
                var item = Request.Headers["Username"];
                return new Regex("[*'\",_&#^@]").Replace(item, string.Empty);
            }
        }

        protected string Subdomain
        {
            get
            {
                var item = Request.Headers["Subdomain"];
                return new Regex("[*'\",_&#^@]").Replace(item, string.Empty);
            }
        }

        protected Guid Company
        {
            get
            {
                var item = Request.Headers["Company"];
                var str1 = new Regex("[*'\",_&#^@]").Replace(item, string.Empty);
                if (Guid.TryParse(str1.ToString(), out Guid company))
                    return company;
                return Guid.Empty;
            }
        }

        public SubActionPermission SubActionPermission { get; set; }

        protected ConnectionModel Connect
        {
            get
            {
                return JsonConvert.DeserializeObject<ConnectionModel>(Request.Headers["Connect"]);
            }
        }
    }

}
