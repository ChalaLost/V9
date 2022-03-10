using System;
using V9Common;

namespace V9ManagerIVR.Models.CRM
{
    public class PermissionModel
    {
        public string Data { get; set; }
        public string Permission
        {
            get
            {
                try
                {
                    return Data.Split(':')[0];
                }
                catch
                {
                    return string.Empty;
                }

            }
        }
        public SubActionPermission SubActionPermission
        {
            get
            {
                try
                {
                    return (SubActionPermission)Convert.ToInt32(Data.Split(':')[1]);
                }
                catch
                {
                    return SubActionPermission.None;
                }
            }
        }
    }
}
