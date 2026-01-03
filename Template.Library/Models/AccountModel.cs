using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Library.Models
{
    public class RequestResetPassword
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public DateTime? PasswordExpiryDate { get; set; } = DateTime.Now.AddDays(60);
        public bool PasswordNeverExpires { get; set; }
    }

    public class RequestChangePassword
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
    public class RequestUpdateAccount
    {
        [Required]
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
    }
    public class RequestRegisterAccount
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string? ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
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
        //[Required]
        //[DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        //[Required]
        //[DataType(DataType.Password)]
        public string  Password { get; set; } = string.Empty;
    }

    public class ResponseLoginAccount
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public List<string>? Claims { get; set; }
    }

    public class RequestUpdateUserClaimModel
    {
        [Required]
        public string ClaimName { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public bool IsAdding { get; set; }
    }

    public class RequestUpdateUserRoleModel
    {
        [Required]
        public string Role { get; set; } = string.Empty;

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
