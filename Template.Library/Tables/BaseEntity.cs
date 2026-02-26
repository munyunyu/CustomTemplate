using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Constants;
using Template.Library.Extensions;

namespace Template.Library.Tables
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public Guid? CreatedById { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public Guid? LastUpdatedById { get; set; }
        public string? Hash { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string GenerateHash()
        {
            var hashInput = this.GetConcatenatedProperties();

            return hashInput.GetSHA256();
        }
        public bool VerifyHash(string? auditHash)
        {
            var currentHash = this.GenerateHash();

            if (currentHash == null || auditHash == null) return false;

            return currentHash == auditHash;
        }

        private string? createdByIdName;

        private string? lastUpdatedByIdName;

        [NotMapped]
        public string? CreatedByIdName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(createdByIdName)) return createdByIdName;
                    return SystemUsers.Admins?.SingleOrDefault(x => x.Id == this.CreatedById.ToString())?.Email;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set { createdByIdName = value; }
        }

        [NotMapped]
        public string? LastUpdatedByIdName
        {
            get
            {
                try
                {
                    if (!string.IsNullOrEmpty(lastUpdatedByIdName)) return lastUpdatedByIdName;
                    return SystemUsers.Admins?.SingleOrDefault(x => x.Id == this.LastUpdatedById.ToString())?.Email;
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            set { lastUpdatedByIdName = value; }
        }

    }

    /// <summary>
    /// Any database table that inherints this interface its table data will be logged to audti trail
    /// </summary>
    public interface IAuditable { }
}
