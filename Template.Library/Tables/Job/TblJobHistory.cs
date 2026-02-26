using Template.Library.Enums;

namespace Template.Library.Tables.Job
{
    public class TblJobHistory : BaseEntity, IAuditable
    {
        public required Guid JobScheduleId { get; set; }
        public required string JobName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public Status Status { get; set; } = Status.InProgress;
        public int AffectedRecords { get; set; } = 0;
        public string? ErrorMessage { get; set; }
        public string? Notes { get; set; }

        public virtual TblJobSchedule? JobSchedule { get; set; }
    }
}
