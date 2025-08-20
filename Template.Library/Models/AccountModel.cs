using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Models
{
    public class RequestRegisterAccount
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
    }

    public class ResponseRegisterAccount
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? Message { get; set; }
    }

    public class RequestLoginAccount
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class ResponseLoginAccount
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RequestUpdateUserClaimModel
    {
        [Required]
        public required string ClaimName { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool IsAdding { get; set; }
    }

    public class RequestUpdateUserRoleModel
    {
        [Required]
        public required string Role { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool IsAdding { get; set; }
    }

    public class ResponseUserRolesAndClaims
    {
        public List<string>? Roles { get; set; }
        public List<string>? Claims { get; set; }
    }
}
