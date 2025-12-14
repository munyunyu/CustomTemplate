using Template.Portal.Interface;
using Template.Portal.Interface.System;

namespace Template.Portal.Services
{
    public class PortalService : IPortalService
    {
        public required IAccountService Account { get; set; }
        public required IAdminService Admin { get; set; }

        public PortalService
        (
            IAccountService Account,
            IAdminService Admin
        )
        {
            this.Account = Account;
            this.Admin = Admin;
        }
    }
}
