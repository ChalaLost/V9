using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace V9ManagerIVR.Models.Company
{
    public class CreateIVRFileCompanyModel
    {
        public string Name { get; set; }
        public string FileName { get; set; }
        public IFormFile File { get; set; }
        public string Content { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class UpdateIVRFileCompanyModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public IFormFile File { get; set; }
    }
}
