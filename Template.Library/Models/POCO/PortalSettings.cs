using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Models.POCO
{
    public class PortalSettings
    {
        /// <summary>
        /// Application base url
        /// </summary>
        public required string AppBaseUrl { get; set; }

        /// <summary>
        /// Api base url
        /// </summary>
        public required string ApiBaseUrl { get; set; }
    }
}
