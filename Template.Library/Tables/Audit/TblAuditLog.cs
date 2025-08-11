using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Tables.Audit
{
    public class TblAuditLog : BaseEntity
    {
        public required Guid EntityId { get; set; }
        public required string EntityName { get; set; }
        public required string Action { get; set; }
        public required string Changes { get; set; }
    }
}
