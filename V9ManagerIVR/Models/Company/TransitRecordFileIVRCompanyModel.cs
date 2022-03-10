using Microsoft.AspNetCore.Http;

namespace V9ManagerIVR.Models.Company
{
    public class TransitRecordFileIVRCompanyModel
    {
        public string CompanyCode { get; set; }
        public string SSH_IP { get; set; }
        public string SSH_Password { get; set; }
        public string SSH_UserName { get; set; }
        public int SSH_Port { get; set; } = 3822;
        public IFormFile File { get; set; }

    }
}
