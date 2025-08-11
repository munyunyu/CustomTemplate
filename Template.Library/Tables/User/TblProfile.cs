using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Tables.User
{
    [Table("TblProfile", Schema = "user")]
    public class TblProfile : TblProfileProp, IAuditable
    {
        public Guid UserId { get; set; }
    }

    public class TblProfileProp : BaseEntity
    {
        public string? Firstname { get; set; }
        public string? Surname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Email { get; set; }
    }
}
