using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Enums
{
    public enum Priority
    {
        [Description("Low Priority")]
        Low = 1,

        [Description("Medium Priority")]
        Medium = 2,

        [Description("High Priority")]
        High = 3,

        [Description("Critical Priority")]
        Critical = 4
    }
}
