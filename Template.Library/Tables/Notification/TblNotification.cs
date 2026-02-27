using System.ComponentModel.DataAnnotations.Schema;
using Template.Library.Enums;

namespace Template.Library.Tables.Notification
{
    [Table("TblNotification", Schema = "comms")]
    public class TblNotification : TblNotificationProp
    {
    }

    public class TblNotificationProp : BaseEntity
    {
        public string? UserId { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public Status Status { get; set; } = Status.Pending;
    }
}
