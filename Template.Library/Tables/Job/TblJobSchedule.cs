using Template.Library.Enums;

namespace Template.Library.Tables.Job
{
    public class TblJobSchedule : BaseEntity, IAuditable
    {
        public required string JobName { get; set; }
        public required string CronExpression { get; set; }
        public bool IsEnabled { get; set; } = true;
        public DateTime? NextRunTime { get; set; }
        public DateTime? LastRunTime { get; set; }
        public Status LastRunStatus { get; set; } = Status.Pending;

        public virtual ICollection<TblJobHistory> History { get; set; } = new List<TblJobHistory>();
    }
}
