using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Tables;

namespace Template.Database.Context
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public Guid? CreatedById { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public Guid? LastUpdatedById { get; set; }
        public string? Hash { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; } = false;

        public DateTime? PasswordExpiryDate { get; set; }
        public bool PasswordNeverExpires { get; set; } = false;
        public DateTime? LastPasswordChangeDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEndDate { get; set; }
        public bool IsManuallyLocked { get; set; }

        public virtual ICollection<TblPasswordHistory> PasswordHistories { get; set; } = new List<TblPasswordHistory>();
    }

    [Table("TblPasswordHistory", Schema = "user")]
    public class TblPasswordHistory : BaseEntity
    {        
        public string? PasswordHash { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
