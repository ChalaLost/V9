using System;

namespace V9ManagerIVR.Models.Company
{
    public class DeleteIVRRecordModel
    {
        public string SSH_IP { get; set; }
        public int SSH_Port = 3822;
        public string SSH_UserName { get; set; }
        public string SSH_Password { get; set; }

        public string FileName { get; set; }
        /// <summary>
        /// Subdomain
        /// </summary>
        public string CompanyCode { get; set; }
    }
}
