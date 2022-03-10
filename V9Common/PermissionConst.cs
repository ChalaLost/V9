using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace V9Common
{
    public static class PermissionConst
    {
        #region Quyền mặc định tài khoản cá nhân
        public const string Default_account = "default-account";
        #endregion

        #region Quản trị tài khoản
        /// <summary>
        /// Cho phép truy cập màn hình quản lý tài khoản
        /// </summary>
        public const string Account_view = "account-view";
        /// <summary>
        /// Cho phép tạo mới tài khoản
        /// </summary>
        public const string Account_add = "account-add";
        /// <summary>
        /// Cho phép cập nhật tài khoản
        /// </summary>
        public const string Account_edit = "account-edit";
        /// <summary>
        /// Cho phép khóa tài khoản
        /// </summary>
        public const string Account_lock = "account-lock";
        /// <summary>
        /// Cho phép xóa tài khoản
        /// </summary>
        public const string Account_delete = "account-delete";
        /// <summary>
        /// Cho phép reset mật khẩu tài khoản
        /// </summary>
        public const string Account_resetpassword = "account-resetpassword";

        #endregion

        #region Quản trị khách hàng
        /// <summary>
        /// Cho phép truy cập màn hình quản lý khách hàng
        /// </summary>
        public const string Company_view = "company-view";
        /// <summary>
        /// Cho phép tạo mới khách hàng
        /// </summary>
        public const string Company_add = "company-add";
        /// <summary>
        /// Cho phép cập nhật khách hàng
        /// </summary>
        public const string Company_edit = "company-edit";
        /// <summary>
        /// Cho phép khóa khách hàng
        /// </summary>
        public const string Company_lock = "company-lock";
        /// <summary>
        /// Cho phép xóa khách hàng
        /// </summary>
        public const string Company_delete = "company-delete";

        #endregion

        #region Quản trị quyền
        /// <summary>
        /// Cho phép tạo mới quyền
        /// </summary>
        public const string Role_add = "role-add";
        /// <summary>
        /// Cho phép truy cập màn hình quản lý quyền
        /// </summary>
        public const string Role_view = "role-view";
        /// <summary>
        /// Cho phép khóa quyền
        /// </summary>
        public const string Role_delete = "role-delete";
        /// <summary>
        /// Cho phép cập nhật thông tin quyền
        /// </summary>
        public const string Role_edit = "role-edit";

        #endregion

        #region Quản trị danh mục
        /// <summary>
        /// Cho phép tạo mới danh mục
        /// </summary>
        public const string Category_add = "category-add";
        /// <summary>
        /// Cho phép truy cập màn hình danh mục
        /// </summary>
        public const string Category_view = "category-view";
        /// <summary>
        /// Cho phép xóa danh mục
        /// </summary>
        public const string Category_delete = "category-delete";
        /// <summary>
        /// Cho phép cập nhật danh mục
        /// </summary>
        public const string Category_edit = "category-edit";

        #endregion

        #region Quản trị sub domain
        /// <summary>
        /// Cho phép tạo mới sub domain
        /// </summary>
        public const string SubDomain_add = "subdomain-add";
        /// <summary>
        /// Cho phép truy cập màn hình sub domain
        /// </summary>
        public const string SubDomain_view = "subdomain-view";
        /// <summary>
        /// Cho phép xóa sub domain
        /// </summary>
        public const string SubDomain_delete = "subdomain-delete";
        /// <summary>
        /// Cho phép cập nhật sub domain
        /// </summary>
        public const string SubDomain_edit = "subdomain-edit";

        #endregion

        #region Quản trị máy chủ
        /// <summary>
        /// Cho phép tạo mới máy chủ
        /// </summary>
        public const string Server_add = "server-add";
        /// <summary>
        /// Cho phép truy cập màn hình máy chủ
        /// </summary>
        public const string Server_view = "server-view";
        /// <summary>
        /// Cho phép xóa máy chủ
        /// </summary>
        public const string Server_delete = "server-delete";
        /// <summary>
        /// Cho phép cập nhật máy chủ
        /// </summary>
        public const string Server_edit = "server-edit";

        #endregion

        #region Quản trị kết nối
        /// <summary>
        /// Cho phép tạo mới kết nối
        /// </summary>
        public const string Network_add = "network-add";
        /// <summary>
        /// Cho phép truy cập màn hình kết nối
        /// </summary>
        public const string Network_view = "network-view";
        /// <summary>
        /// Cho phép xóa kết nối
        /// </summary>
        public const string Network_delete = "network-delete";
        /// <summary>
        /// Cho phép cập nhật kết nối
        /// </summary>
        public const string Network_edit = "network-edit";
        /// <summary>
        /// Cho phép cấu hình ssh đến server domain
        /// </summary>
        public const string domain_config_add = "domain-config-add";
        /// <summary>
        /// Cho phép cập nhật cấu hình ssh đến server domain
        /// </summary>
        public const string domain_config_update = "domain-config-update";
        #endregion

        #region Quản trị kênh truyền
        /// <summary>
        /// Cho phép tạo mới kênh truyền
        /// </summary>
        public const string Channel_add = "channel-add";
        /// <summary>
        /// Cho phép truy cập màn hình kênh truyền
        /// </summary>
        public const string Channel_view = "channel-view";
        /// <summary>
        /// Cho phép xóa kênh truyền
        /// </summary>
        public const string Channel_delete = "channel-delete";
        /// <summary>
        /// Cho phép cập nhật kênh truyền
        /// </summary>
        public const string Channel_edit = "channel-edit";

        #endregion
    }
}
