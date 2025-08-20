using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Tables.Notification
{
    [Table("TblEmailTemplate", Schema = "comms")]
    public class TblEmailTemplate : TblEmailTemplateProp
    {
    }

    public class TblEmailTemplateProp : BaseEntity
    {
        public string? Name { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
