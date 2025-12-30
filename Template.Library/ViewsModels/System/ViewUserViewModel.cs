using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Tables;

namespace Template.Library.ViewsModels.System
{
    public class ViewUserViewModel :BaseEntity
    {
        public new string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
    }

    public class ApplicationUserViewModel : BaseEntity
    {
        public new string? Id { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? PasswordExpiryDate { get; set; }
        public bool PasswordNeverExpires { get; set; } = false;
        public DateTime? LastPasswordChangeDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public bool IsManuallyLocked { get; set; }
        public string? PhoneNumber { get; set; }

        public virtual bool EmailConfirmed { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

        public List<string>? Roles { get; set; }
        public List<string>? Claims { get; set; }
    }

}
