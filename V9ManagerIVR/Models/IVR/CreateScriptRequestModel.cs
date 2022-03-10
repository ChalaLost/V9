using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using V9Common;

namespace V9ManagerIVR.Models.IVR
{
    public class CreateScriptRequestModel
    {
        public Guid ParentId { get; set; }
        public V9_IVRLevel Level { get; set; }
        public string Name { get; set; }
        public Action Action { get; set; }
        public List<CreateScriptRequestModel> Childrens { get; set; }
    }
}
