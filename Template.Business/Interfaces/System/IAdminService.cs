using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Models;
//using Template.Library.Request;
using Template.Library.ViewsModels.System;

namespace Template.Business.Interfaces.System
{
    public interface IAdminService
    {
        Task<IEnumerable<ViewUserViewModel>?> GetSystemUsersAsync();
        Task<IEnumerable<SystemUserRolesViewModel>?> GetUsersRolesAsync();
    }
}
