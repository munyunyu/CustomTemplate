using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces;
using Template.Business.Interfaces.Profile;
using Template.Business.Interfaces.System;

namespace Template.Business.Services
{
    public class PortalService : IPortalService
    {
       
        //System
        public required IAccountService Account { get; set; }
        public required ICommunicationService Communication { get; set; }

        //Other  
        public required IAdminService Admin { get; set; }

        //Profile
        public required IProfileService Profile { get; set; }

        public PortalService
        (
            //IEducationService Education,

            //System
            IAccountService Account,
            ICommunicationService Communication,
            IAdminService Admin,

            IProfileService Profile
            )
        {
            //System
            this.Account = Account;
            this.Communication = Communication;
            this.Admin = Admin;            

            //Profile
            this.Profile = Profile;
        }
    }
}
