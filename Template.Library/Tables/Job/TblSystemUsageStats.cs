using System.ComponentModel.DataAnnotations.Schema;

namespace Template.Library.Tables.Job
{
    [Table("TblSystemUsageStats")]
    public class TblSystemUsageStats : BaseEntity, IAuditable
    {
        public DateTime SnapshotDate { get; set; }
        public int TotalUsers { get; set; }
        public int ConfirmedUsers { get; set; }
        public int LockedUsers { get; set; }
        public int PendingEmails { get; set; }
        public int FailedEmails24h { get; set; }
        public int JobsRun24h { get; set; }
        public int FailedJobs24h { get; set; }
    }
}
