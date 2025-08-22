using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Constants
{
    public class SystemClaims
    {
        //Admin claims
        public const string AdminCreate = "AdminCreate";
        public const string AdminUpdate = "AdminUpdate";
        public const string AdminDelete = "AdminDelete";
        public const string AdminRead = "AdminRead";

        //Profile claims
        public const string ProfileCreate = "ProfileCreate";
        public const string ProfileEdit = "ProfileEdit";
        public const string ProfileDelete = "ProfileDelete";
        public const string ProfileView = "ProfileView";

    }
}
