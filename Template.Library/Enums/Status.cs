using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Enums
{
    public enum Status
    {
        Success = 0,        // Successfully finished
        Draft = 1,          // Initial creation, not finalized
        Active = 2,         // Currently active and working
        InProgress = 3,     // Being worked on
        OnHold = 4,         // Temporarily paused
        Pending = 5,        // Waiting to be processed
        Cancelled = 6,      // Intentionally stopped
        Failed = 7,         // Ended with errors
        Archived = 8,       // Stored for records, not active
        Deleted = 9         // Marked for deletion   
    }
}
