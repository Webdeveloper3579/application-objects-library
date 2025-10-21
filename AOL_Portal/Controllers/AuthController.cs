using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using AOL_Portal.Configuration;
using AOL_Portal.Data;
using AOL_Portal.Models;
using AOL_Portal.Services;
using static Google.Cloud.RecaptchaEnterprise.V1.TransactionData.Types;

namespace AOL_Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AolApplicationUser> _userManager;
        private readonly SignInManager<AolApplicationUser> _signInManager;
        private readonly RoleManager<AolUserRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly ApplicationConfigService _applicationConfigService;
        private readonly AOLContext _context;
        
        public AuthController(
            UserManager<AolApplicationUser> userManager,
            SignInManager<AolApplicationUser> signInManager,
            RoleManager<AolUserRole> roleManager,
            IEmailService emailService,
            ApplicationConfigService applicationConfigService,
            IJwtService jwtService,
            AOLContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _emailService = emailService;
            _applicationConfigService = applicationConfigService;
            _context = context;
        }

        /// <summary>
        /// Simple test endpoint
        /// </summary>
        [HttpGet("test")]
        public ActionResult Test()
        {
            return Ok(new { Message = "Backend is working!", Timestamp = DateTime.UtcNow });
        }

        [HttpPost("sign-in")]
        public async Task<ActionResult<AuthResponse>> SignIn([FromBody] LoginRequest request)
        {
            Console.WriteLine($"=== SIGN-IN DEBUG START ===");
            Console.WriteLine($"Email: '{request.Email}'");
            Console.WriteLine($"Password: '{request.Password}'");
            Console.WriteLine($"Email Length: {request.Email?.Length}");
            Console.WriteLine($"Password Length: {request.Password?.Length}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ERROR: Invalid request data");
                return BadRequest(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid request data" 
                });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                Console.WriteLine($"ERROR: User not found for email: {request.Email}");
                return Unauthorized(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid email or password" 
                });
            }

            Console.WriteLine($"User found: {user.Email}, StatusId: {user.StatusId}, EmailConfirmed: {user.EmailConfirmed}");

            // Check password authentication
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            Console.WriteLine($"Password check result: {result.Succeeded}");
            
            if (!result.Succeeded)
            {
                Console.WriteLine($"Authentication failed for user: {request.Email}");
                return Unauthorized(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid email or password" 
                });
            }

            if(user.StatusId==1)
            {
                Console.WriteLine($"ERROR: Account email has not been validated (StatusId: {user.StatusId})");
                return Unauthorized(new AuthResponse
                {
                    Success = false,
                    Message = "Account email has not been validated"
                });
            }
            else if (user.StatusId != 2) // Assuming 2 is active status
            {
                Console.WriteLine($"ERROR: Account is not active (StatusId: {user.StatusId})");
                return Unauthorized(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Account is not active" 
                });
            }

            Console.WriteLine($"Authentication successful! Generating tokens...");
            var authResponse = await _jwtService.GenerateTokenAsync(user);
            
            // Store the refresh token for this user
            await _userManager.SetAuthenticationTokenAsync(
                user,
                TokenOptions.DefaultProvider,
                "RefreshToken",
                authResponse.RefreshToken);
            
            Console.WriteLine($"=== SIGN-IN DEBUG END - SUCCESS ===");
            return Ok(authResponse);
        }

        /// <summary>
        /// The user will receive an url that will be {siteurl}/RegisterConfirmation/token?xxxxxx
        /// The Client application will have this page available and use the token send to confirm registration with this API Call
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("registrationConfirmation")]
        public async Task<ActionResult<ValidateResult>> RegisterConfirmation(string token)
        {
            ValidateResult returnResult = new ValidateResult() { Success = false };
            if (string.IsNullOrEmpty(token))
            {
                returnResult.Message = "Invalid Token";
                return Ok(returnResult);
            }
              

            try
            {
               
                string decrypt = CryptoClass.Decrypt(_applicationConfigService.ApplicationConfigSettings.CryptoKey, token);
                AuthModel auth = JsonConvert.DeserializeObject<AuthModel>(decrypt);
                var userValidate = await _userManager.FindByNameAsync(auth.Username);
                if (!userValidate.EmailConfirmed)
                {
                    var confirm = await _userManager.ConfirmEmailAsync(userValidate, auth.EmailConfirmToken);
                    if (confirm.Succeeded)
                    {
                        returnResult.Success = true;
                        var resetpwdToken = await _userManager.GeneratePasswordResetTokenAsync(userValidate);

                        userValidate.StatusId = 2;
                        var result = await _userManager.UpdateAsync(userValidate);
                        if (result.Succeeded)
                        {
                            returnResult.Message = "";
                            returnResult.Success = true;
                            await _emailService.SendMailResetPassword(userValidate.Email, userValidate.FirstName + ' ' + userValidate.Surname, resetpwdToken);
                        }
                        else
                        {
                            returnResult.Message = "Cannot update email validation. Please contact support";
                            returnResult.Success = false;
                        }
                    }
                    else
                    {
                        returnResult.Success = false;
                        StringBuilder errorbuilder = new StringBuilder();
                        foreach (var error in confirm.Errors)
                        {
                            errorbuilder.Append(error.Description);
                            errorbuilder.Append("< /br>");
                        }
                        returnResult.Message = errorbuilder.ToString();
                    }
                }
                else { returnResult.Success = false; returnResult.Message = "Email Already Confirmed"; }
                return Ok(returnResult);
            }
            catch (Exception ex)
            {
                returnResult.Success = false;
                returnResult.Message = "Unexpected Server Error";
                return BadRequest(returnResult);
            }
           
        }

        [Authorize(Roles = "UserAdmin")]
        [HttpPost("resendEmailConfirmationCode")]
        public async Task<ActionResult<ValidateResult>> ResendEmailConfirmationCode(EmailConfirmationRequest emailConfirmationRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(emailConfirmationRequest.Email);
            if (existingUser == null || !existingUser.Id.Equals(emailConfirmationRequest.UserId, StringComparison.OrdinalIgnoreCase))
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "Invalid User"
                });
            }

            // Check if user's email is already confirmed
            if (existingUser.EmailConfirmed)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "Email is already confirmed"
                });
            }

            try
            {
                // Generate new email confirmation token (this will invalidate any existing tokens)
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(existingUser);
                
                // Create the encrypted token for email
                AuthModel model = new AuthModel() { Username = existingUser.Email, EmailConfirmToken = emailToken };
                var serialize = JsonConvert.SerializeObject(model);
                var encrypted = CryptoClass.Encrypt(_applicationConfigService.ApplicationConfigSettings.CryptoKey, serialize);
                
                // Send the confirmation email
                await _emailService.SendMailRegistrationConfirmation(
                    _applicationConfigService.ApplicationConfigSettings.CallbackUrl, 
                    existingUser.Email, 
                    existingUser.FirstName + ' ' + existingUser.Surname, 
                    encrypted);

                return Ok(new ValidateResult()
                {
                    Success = true,
                    Message = "Email confirmation code resent successfully"
                });
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "Failed to send confirmation email. Please try again later."
                });
            }
        }
        /// <summary>
        /// An admin user will create a new user, and the Register API will be called
        /// A validation Result will be returned indicating if the registration was successfull
        /// The registration process will also send an email to the user, with an email validation link
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "UserAdmin")]
        [HttpPost("register")]
        public async Task<ActionResult<ValidateResult>> Register(RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return  BadRequest( new ValidateResult
                { 
                    Success = false, 
                    Message = "Invalid request" 
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new ValidateResult()
                { 
                    Success = false, 
                    Message = "User with this email already exists" 
                };
            }

            var user = new AolApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                Surname = request.Surname,
                CreatedDtm = DateTime.UtcNow,
                LastUpdateDtm = DateTime.UtcNow,
                LastUpdateUserId = "system",
                StatusId = 1, // Active
                EmailConfirmed = false                
            };

            var result = await _userManager.CreateAsync(user, "UMASD3f@ult98712#");
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new AuthResponse 
                { 
                    Success = false, 
                    Message = errors 
                });
            }
            
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            AuthModel model = new AuthModel() { Username = user.Email, EmailConfirmToken = emailToken };
            var serialize = JsonConvert.SerializeObject(model);
            var encrypted = CryptoClass.Encrypt(_applicationConfigService.ApplicationConfigSettings.CryptoKey, serialize);
            await _emailService.SendMailRegistrationConfirmation(_applicationConfigService.ApplicationConfigSettings.CallbackUrl, request.Email, request.FirstName + ' ' + request.Surname, encrypted);
            return Ok(new ValidateResult()
            {
                Success = true,
                Message = "User Created and Registration Email Send"
            });

        }

        /// <summary>
        /// Public sign-up endpoint for new users
        /// Creates a user account with email/password authentication
        /// </summary>
        [HttpPost("sign-up")]
        public async Task<ActionResult<ValidateResult>> SignUp(SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                { 
                    Success = false, 
                    Message = "Invalid request" 
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new ValidateResult()
                { 
                    Success = false, 
                    Message = "User with this email already exists" 
                });
            }

            // Create the user
            var user = new AolApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.Name,
                Surname = request.Company ?? string.Empty,
                CreatedDtm = DateTime.UtcNow,
                LastUpdateDtm = DateTime.UtcNow,
                LastUpdateUserId = "system",
                StatusId = 2, // Active - no confirmation required
                EmailConfirmed = true                
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BadRequest(new ValidateResult
                { 
                    Success = false, 
                    Message = errors 
                });
            }

            Console.WriteLine($"User created successfully: {user.Email}");
            
            // Create default customer records for the new user
            try
            {
                await CreateDefaultCustomerRecords(user.Id);
                Console.WriteLine($"Default customer records created for user: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to create default customer records: {ex.Message}");
                // Don't fail the sign-up if customer records creation fails
            }
            
            return Ok(new ValidateResult()
            {
                Success = true,
                Message = "Account created successfully. You can now sign in."
            });
        }

        /// <summary>
        /// Creates default customer records for a new user with standard custom fields
        /// </summary>
        private async Task CreateDefaultCustomerRecords(string userId)
        {
            // Get all standard custom fields
            var standardFields = await _context.AolCustomerCustomFields
                .Where(cf => cf.CustomerCustomType == "Standard")
                .ToListAsync();

            if (!standardFields.Any())
            {
                Console.WriteLine("No standard custom fields found. Skipping customer record creation.");
                return;
            }

            var customerRecords = new List<AspNetCustomer>();

            foreach (var field in standardFields)
            {
                var customerRecord = new AspNetCustomer
                {
                    CustomerId = userId,
                    CustomFieldId = field.CustomerFieldId,
                    CustomFieldValue = GetDefaultValueForField(field.CustomerCustomFieldName),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = null
                };

                customerRecords.Add(customerRecord);
            }

            _context.AspNetCustomers.AddRange(customerRecords);
            await _context.SaveChangesAsync();
        }

        private string GetDefaultValueForField(string fieldName)
        {
            return fieldName.ToLower() switch
            {
                "customername" => "",
                "customertype" => "",
                "address" => "",
                _ => ""
            };
        }

        [HttpPost("sign-in-with-token")]
        public async Task<ActionResult<AuthResponse>> SignInWithToken([FromBody] SignInWithTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid request data" 
                });
            }

            try
            {
                // Validate the JWT token
                var principal = _jwtService.ValidateToken(request.AccessToken);
                if (principal == null)
                {
                    return Unauthorized(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "Invalid token" 
                    });
                }

                // Get user ID from token
                var userId = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "Invalid token" 
                    });
                }

                // Get user from database
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Unauthorized(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "User not found" 
                    });
                }

                // Check if user is still active
                if (user.StatusId != 2)
                {
                    return Unauthorized(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "User account is not active" 
                    });
                }

                // Generate new tokens
                var authResponse = await _jwtService.GenerateTokenAsync(user);
                
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid token" 
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> RefreshToken(RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Invalid refresh token" 
                });
            }

            try
            {
                // Get the stored refresh token from the database
                // For this implementation, we'll need to find the user by checking all users
                // In a production system, you might want to store refresh tokens in a separate table
                var users = _userManager.Users.ToList();
                AolApplicationUser userWithToken = null;
                string storedToken = null;

                foreach (var user in users)
                {
                    var token = await _userManager.GetAuthenticationTokenAsync(
                        user,
                        TokenOptions.DefaultProvider,
                        "RefreshToken");
                    
                    if (token == request.RefreshToken)
                    {
                        userWithToken = user;
                        storedToken = token;
                        break;
                    }
                }

                if (userWithToken == null || storedToken != request.RefreshToken)
                {
                    return BadRequest(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "Invalid refresh token" 
                    });
                }

                // Check if user is still active
                if (userWithToken.StatusId != 2)
                {
                    return BadRequest(new AuthResponse 
                    { 
                        Success = false, 
                        Message = "User account is not active" 
                    });
                }

                // Generate new tokens
                var authResponse = await _jwtService.GenerateTokenAsync(userWithToken);
                
                // Update the stored refresh token
                await _userManager.SetAuthenticationTokenAsync(
                    userWithToken,
                    TokenOptions.DefaultProvider,
                    "RefreshToken",
                    authResponse.RefreshToken);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new AuthResponse 
                { 
                    Success = false, 
                    Message = "Error refreshing token: " + ex.Message 
                });
            }
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
           
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var userInfo = new UserInfo
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                Surname = user.Surname,
                IsSiteAdmin = user.IsSiteAdmin,
                Roles = userRoles.ToList()
            };

            return Ok(userInfo);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                // Get the current user
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not found" });
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                // Sign out the user
                await _signInManager.SignOutAsync();

                // Invalidate all refresh tokens for this user
                // Remove any stored refresh tokens
                await _userManager.RemoveAuthenticationTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    "RefreshToken");

                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error during logout", error = ex.Message });
            }
        }

        /// <summary>
        /// This procedure is called when the user reset's a password or its created after registration
        /// The user would have received an email containing a verification code for a password reset, 
        /// </summary>
        /// <param name="resetPasswordRequest"></param>
        /// <returns></returns>
        [HttpPost("resetpassword")]
        public async Task<ActionResult<ValidateResult>> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordRequest.Email);
            if (user == null)
            {
                return Ok(new ValidateResult() {
                     
                     Message="No user found",
                     Success = false
                 });
                }
            var storedCode = await _userManager.GetAuthenticationTokenAsync(
                   user,
                   TokenOptions.DefaultProvider,
                   "ResetCode");

            if (storedCode != resetPasswordRequest.VerificationCode)
            {
                return Ok(new ValidateResult()
                {

                    Message = "Invalid Verification Code",
                    Success = false
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, resetPasswordRequest.NewPassword);
            if (result.Succeeded)
            {
                // Remove the verification code
                await _userManager.RemoveAuthenticationTokenAsync(
                    user,
                    TokenOptions.DefaultProvider,
                    "ResetCode");

                return Ok(new ValidateResult()
                {

                    Message = "Password updated",
                    Success = true
                });
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                foreach (var error in result.Errors)
                {
                    builder.AppendLine(error.Description);
                }
                return Ok(new ValidateResult()
                {

                    Message = "Error",
                    Success = false,
                    Errors = (result.Errors.Select(c=>c.Description)).ToList()
                });
            }
        }
        /// <summary>
        /// This is when a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest email)
        {
            try {
                var user = await _userManager.FindByEmailAsync(email.Email);
                if (user == null) { 
                    return NotFound();
                }
                var verificationCode = await _userManager.GenerateUserTokenAsync(
                          user,
                         TokenOptions.DefaultProvider,
                          "ResetPassword");
                await _userManager.SetAuthenticationTokenAsync(
                       user,
                       TokenOptions.DefaultProvider,
                       "ResetCode",
                       verificationCode);
                await _emailService.SendMailResetPassword(email.Email,user.FirstName + ' ' + user.Surname,verificationCode);

                return Ok();
            }
            catch (Exception ex) 
            { 
                return BadRequest("Email send failed");
            }
        }

        /// <summary>
        /// Unlock a user account that has been locked out due to failed login attempts
        /// </summary>
        /// <param name="request">Contains the UserId to unlock</param>
        /// <returns></returns>
        [Authorize(Roles = "UserAdmin")]
        [HttpPost("unlockuser")]
        public async Task<ActionResult<ValidateResult>> UnlockUser(UnlockUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                // Check if user is actually locked out
                var isLockedOut = await _userManager.IsLockedOutAsync(user);
                if (!isLockedOut)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "User is not currently locked out"
                    });
                }

                // Unlock the user by setting lockout end date to null
                var result = await _userManager.SetLockoutEndDateAsync(user, null);
                if (result.Succeeded)
                {
                    // Reset the access failed count
                    await _userManager.ResetAccessFailedCountAsync(user);

                    return Ok(new ValidateResult()
                    {
                        Success = true,
                        Message = $"User {user.Email} has been unlocked successfully"
                    });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Failed to unlock user: {errors}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "An error occurred while unlocking the user"
                });
            }
        }

        /// <summary>
        /// Update user information (Admin only)
        /// </summary>
        /// <param name="request">Contains the user information to update</param>
        /// <returns></returns>
        [Authorize(Roles = "UserAdmin")]
        [HttpPut("updateuser")]
        public async Task<ActionResult<ValidateResult>> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                // Check if email is being changed and if it's already taken by another user
                if (user.Email != request.Email)
                {
                    var existingUserWithEmail = await _userManager.FindByEmailAsync(request.Email);
                    if (existingUserWithEmail != null && existingUserWithEmail.Id != request.UserId)
                    {
                        return Ok(new ValidateResult()
                        {
                            Success = false,
                            Message = "Email is already taken by another user"
                        });
                    }
                }

                // Update user properties
                user.Email = request.Email;
                user.UserName = request.Email; // Keep UserName in sync with Email
                user.FirstName = request.FirstName;
                user.Surname = request.Surname;
                user.StatusId = request.StatusId;
                //user.IsSiteAdmin = request.IsSiteAdmin; never updated set in the backend
                user.LastUpdateDtm = DateTime.UtcNow;
                user.LastUpdateUserId = User.Identity.Name ?? "System"; // Get current user from token

                // Update the user
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = true,
                        Message = $"User {user.Email} has been updated successfully"
                    });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Failed to update user: {errors}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "An error occurred while updating the user"
                });
            }
        }

        /// <summary>
        /// Add a role to a user (Site Admin only)
        /// </summary>
        /// <param name="request">Contains the user ID, role name, and requesting user ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("adduserrole")]
        public async Task<ActionResult<ValidateResult>> AddUserRole( AddUserRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                // Get the requesting user to check if they are a site admin
                var requestingUser = await _userManager.FindByIdAsync(request.RequestingUserId);
                if (requestingUser == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Requesting user not found"
                    });
                }

                // Check if the requesting user is a site admin
                if (!requestingUser.IsSiteAdmin)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Only site administrators can add roles to users"
                    });
                }

                // Get the target user
                var targetUser = await _userManager.FindByIdAsync(request.UserId);
                if (targetUser == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Target user not found"
                    });
                }

                // Check if the role exists
                var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
                if (!roleExists)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Role '{request.RoleName}' does not exist"
                    });
                }

                // Check if user already has this role
                var userRoles = await _userManager.GetRolesAsync(targetUser);
                if (userRoles.Contains(request.RoleName))
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"User already has the role '{request.RoleName}'"
                    });
                }

                // Add the role to the user
                var result = await _userManager.AddToRoleAsync(targetUser, request.RoleName);
                if (result.Succeeded)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = true,
                        Message = $"Role '{request.RoleName}' has been successfully added to user {targetUser.Email}"
                    });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Failed to add role: {errors}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "An error occurred while adding the role to the user"
                });
            }
        }

        /// <summary>
        /// Create a new role (Site Admin only)
        /// </summary>
        /// <param name="request">Contains the role name and requesting user ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("createrole")]
        public async Task<ActionResult<ValidateResult>> CreateRole(CreateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                // Get the requesting user to check if they are a site admin
                var requestingUser = await _userManager.FindByIdAsync(request.RequestingUserId);
                if (requestingUser == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Requesting user not found"
                    });
                }

                // Check if the requesting user is a site admin
                if (!requestingUser.IsSiteAdmin)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Only site administrators can create roles"
                    });
                }

                // Check if the role already exists
                var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
                if (roleExists)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Role '{request.RoleName}' already exists"
                    });
                }

                // Create the new role
                var newRole = new AolUserRole
                {
                    Name = request.RoleName,
                    NormalizedName = request.RoleName.ToUpperInvariant()
                };

                var result = await _roleManager.CreateAsync(newRole);
                if (result.Succeeded)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = true,
                        Message = $"Role '{request.RoleName}' has been created successfully"
                    });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Failed to create role: {errors}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "An error occurred while creating the role"
                });
            }
        }

        /// <summary>
        /// Remove a role from a user (Site Admin only)
        /// </summary>
        /// <param name="request">Contains the user ID, role name, and requesting user ID</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("removeuserrole")]
        public async Task<ActionResult<ValidateResult>> RemoveUserRole(RemoveUserRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ValidateResult
                {
                    Success = false,
                    Message = "Invalid request data"
                });
            }

            try
            {
                // Get the requesting user to check if they are a site admin
                var requestingUser = await _userManager.FindByIdAsync(request.RequestingUserId);
                if (requestingUser == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Requesting user not found"
                    });
                }

                // Check if the requesting user is a site admin
                if (!requestingUser.IsSiteAdmin)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "Only site administrators can remove user roles"
                    });
                }

                // Get the user to remove the role from
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = "User not found"
                    });
                }

                // Check if the role exists
                var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
                if (!roleExists)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Role '{request.RoleName}' does not exist"
                    });
                }

                // Check if the user has the role
                var userHasRole = await _userManager.IsInRoleAsync(user, request.RoleName);
                if (!userHasRole)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"User does not have the role '{request.RoleName}'"
                    });
                }

                // Remove the role from the user
                var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
                if (result.Succeeded)
                {
                    return Ok(new ValidateResult()
                    {
                        Success = true,
                        Message = $"Role '{request.RoleName}' has been removed from user successfully"
                    });
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return Ok(new ValidateResult()
                    {
                        Success = false,
                        Message = $"Failed to remove role: {errors}"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ValidateResult()
                {
                    Success = false,
                    Message = "An error occurred while removing the role from the user"
                });
            }
        }
    }
} 