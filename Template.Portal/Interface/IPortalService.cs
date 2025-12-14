using Template.Portal.Interface.System;

namespace Template.Portal.Interface
{
    public interface IPortalService
    {
        public IAccountService Account { get; set; }
        public IAdminService Admin { get; set; }
    }
}
