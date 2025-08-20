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
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Port { get; set; }
    }
}
