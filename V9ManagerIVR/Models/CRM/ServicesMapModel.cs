using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9ManagerIVR.Models.CRM
{
    public class ServicesMapModel
    {
        /// <summary>
        /// Services kết nối đến asterisk
        /// </summary>
        public string V9AsteriskConnect { get; set; }
        /// <summary>
        /// Services quản trị v9
        /// </summary>
        public string V9ManagerAPI { get; set; }
    }
}
