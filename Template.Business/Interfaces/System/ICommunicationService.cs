using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Business.Interfaces.System
{
    public interface ICommunicationService
    {
        Task SendConfirmEmailAsync(string to, string template_name);
    }
}
