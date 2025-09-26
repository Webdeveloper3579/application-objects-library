using System.ComponentModel.DataAnnotations;

namespace AOL_Portal.Models
{

    public class AuthModel
    {
        public string Username { get; set; }
        public string EmailConfirmToken { get; set; }

    }
    public class ValidateResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public string Message { get; set; }

    }
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string Surname { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
        public UserInfo User { get; set; }
        public string Message { get; set; }
    }

    public class UserInfo
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string FullName => $"{FirstName} {Surname}";
        public bool IsSiteAdmin { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }

    public class EmailConfirmationRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string UserId { get; set; }
    }
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
    
    public class UnlockUserRequest
    {
        [Required]
        public string UserId { get; set; }
    }
    
    public class UpdateUserRequest
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string Surname { get; set; }
        
        public int StatusId { get; set; }
        
        public bool IsSiteAdmin { get; set; }
    }
    
    public class AddUserRoleRequest
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string RoleName { get; set; }
        
        [Required]
        public string RequestingUserId { get; set; }
    }
    
    public class CreateRoleRequest
    {
        [Required]
        public string RoleName { get; set; }
        
        [Required]
        public string RequestingUserId { get; set; }
    }
    
    public class RemoveUserRoleRequest
    {
        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string RoleName { get; set; }
        
        [Required]
        public string RequestingUserId { get; set; }
    }
    
    public class ResetPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
} 