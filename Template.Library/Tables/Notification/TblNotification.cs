using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Tables.Notification
{
    [Table("TblNotification", Schema = "comms")]
    public class TblNotification : TblNotificationProp
    {
    }

    public class TblNotificationProp : BaseEntity
    {
    }
}
