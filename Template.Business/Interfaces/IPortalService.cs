using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.Profile;
using Template.Business.Interfaces.System;

namespace Template.Business.Interfaces
{
    public interface IPortalService
    {
        //System
        public IAccountService Account { get; set; }
        public ICommunicationService Communication { get; set; }

        //Other        
        public IAdminService Admin { get; set; }

        public IProfileService Profile { get; set; }
        public IRabbitMQService Rabbit { get; set; }


    }
}
