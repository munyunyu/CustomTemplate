using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Models
{
    public class RequestCreateProfileModel
    {
        [Required]
        public required string? UserId { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Phonenumber { get; set; }

        [Required]
        public string? Surname { get; set; }
    }

    public class RequestUpdateProfileModel
    {
        [Required]
        public Guid ProfileId { get; set; }

        [Required]
        public string? Description { get; set; }

        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Phonenumber { get; set; }

        [Required]
        public string? Surname { get; set; }
    }
}
