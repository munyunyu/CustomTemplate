using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Enums
{
    public enum Status
    {
        Success = 200,
        NotFound = 404,
        ServerError = 500,
        Failed = -1
    }
}
