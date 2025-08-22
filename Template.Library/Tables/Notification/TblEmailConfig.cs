using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Tables.Notification
{
    [Table("TblEmailConfig", Schema = "comms")]
    public class TblEmailConfig : TblEmailConfigProp
    {
    }

    public class TblEmailConfigProp : BaseEntity
    {
        public string? Name { get; set; }
        public string? SmtpUser { get; set; }
        public string? SmtpPassword { get; set; }
        public int SmtpPort { get; set; }
        public string? SmtpServer { get; set; }
        public bool SmtpEnableSsl { get; set; }
    }
}
