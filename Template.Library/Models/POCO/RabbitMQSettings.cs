using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Models.POCO
{
    public class RabbitMQSettings
    {
        public required string RabbitMQUrl { get; set; }
        public required string VirtualHost { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public int Port { get; set; }
    }
}
