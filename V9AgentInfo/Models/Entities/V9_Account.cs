using System;
using System.Collections.Generic;
using V9MAgentInfo.Models.Entities;

namespace V9AgentInfo.Models.Entities
{
    public class V9_Account : DefaultEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public DateTime? DOB { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }


        public virtual ICollection<V9_AccountRole> AccountRole { get; set; }
    }

    public class V9_Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<V9_AccountRole> AccountRole { get; set; }
        public virtual ICollection<V9_RolePermission> RolePermission { get; set; }
    }

    public class V9_AccountRole
    {
        public Guid AccountId { get; set; }
        public Guid RoleId { get; set; }

        public virtual V9_Account Account { get; set; }
        public virtual V9_Role Role { get; set; }
    }

    public class V9_Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid GroupId { get; set; }
        public string Description { get; set; }
        public virtual ICollection<V9_RolePermission> RolePermission { get; set; }
        public virtual V9_GroupPermission Group { get; set; }
    }

    public class V9_RolePermission
    {
        public Guid RoleId { get; set; }
        public Guid PermissionId { get; set; }

        public virtual V9_Role Role { get; set; }
        public virtual V9_Permission Permission { get; set; }
    }

    public class V9_GroupPermission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<V9_Permission> Permissions { get; set; }
    }

    public class V9_RecoverPassword
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Totp { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsSent { get; set; }
        public bool SendSuccess { get; set; }
        public bool IsHandler { get; set; }
        public DateTime? HandlerDate { get; set; }
        public DateTime ExpiredDate { get; set; }
    }

}
