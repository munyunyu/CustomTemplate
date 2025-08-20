using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Enums;

namespace Template.Library.Tables.Notification
{
    [Table("TblEmailQueue", Schema = "comms")]
    public class TblEmailQueue : BaseEntity
    {
    }

    public class TblEmailQueueProp : BaseEntity
    {
        public string? FromEmailAddress { get; set; }
        public string? ToEmailAddresses { get; set; }
        public string? CCEmailAddresses { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public int SendAttempts { get; set; }
    }
}
