using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Tables;

namespace Template.Library.Views.System
{
    public class ViewSystemUserRoles : BaseEntity
    {
        public new string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? RoleId { get; set; }

    }
}
