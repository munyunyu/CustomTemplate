using Template.Library.ViewsModels.System;

namespace Template.Portal.Interface.System
{
    public interface IAdminService
    {
        Task<IEnumerable<ViewUserViewModel>> GetAllUsersAsnyc();
    }
}
