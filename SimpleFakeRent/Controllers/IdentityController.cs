using Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Identity.Requests;
using Identity.Response;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Identity.Domain;
using EmailService;
using Contracts;
using System.Diagnostics.CodeAnalysis;

namespace SimpleFakeRent.Controllers
{
    [Route("api/Identity")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly IEmailEmmiter _emailEmmiter;
        private readonly ILoggerManager _logger;

        public IdentityController(IIdentityService identityService, IEmailEmmiter emailEmmiter, ILoggerManager logger)
        {
            _identityService = identityService;
            _emailEmmiter = emailEmmiter;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("LandlordRegister")]
        public async Task<IActionResult> LandlordRegister([FromBody] UserRegistrationRequest request)
        {
            AuthenticationResult authResponse = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                if (authResponse.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                }
                else
                {
                    return BadRequest(authResponse.Errors.ToList()[0]);
                }
                
            }

            return Ok(new AuthenticationSuccessResponse
            {
                //Id = authResponse.Id,
                Token = authResponse.Token
            });
        }

        [Authorize(Policy = "Landlord")]
        [HttpPost("TenantRegister")]
        public async Task<IActionResult> TenantRegister([FromBody] UserRegistrationRequest request)
        {
            AuthenticationResult authResponse = await _identityService.TenantRegisterAsync(request.Email, request.Password);

            if (!authResponse.Success)
            {
                if (authResponse.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                }
                else
                {
                    return BadRequest(authResponse.Errors.ToList()[0]);
                }
                
            }

            return Ok(new AuthenticationSuccessResponse
            {
                //Id = authResponse.Id,
                Token = authResponse.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginRequest request)
        {
            AuthenticationResult authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            
            if (!authResponse.Success)
            {
                if(authResponse.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                } 
                else
                {
                    return BadRequest(authResponse.Errors.ToList()[0]);
                }
                
            }
            return Ok(new AuthenticationSuccessResponse
            {

                //Id = authResponse.Id,
                Token = authResponse.Token
            });
        }

        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest forgotPassword)
        {
            if (forgotPassword.Email == null)
            {
                return NotFound("Enter valid email");
            }

            ResetPasswordResult authResult = await _identityService.ForgotPassword(forgotPassword.Email);

            if (authResult.Success)
            {
                string callback = /*Url.Action("ResetPassword", "Identity", new {*/ authResult.Token/*, authResult.Email })*/;
                Email email = new Email(authResult.Email, "Password reset", callback);

                await _emailEmmiter.SendResetEmail(email);

                return Ok();
            }
            else
            {
                if (authResult.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = authResult.Errors
                    });
                }
                else
                {
                    return BadRequest(authResult.Errors.ToList()[0]);
                }
            }
        }

        [AllowAnonymous]
        [HttpPost("EmailPasswordReset")]
        public async Task<IActionResult> EmailPasswordReset(ResetPasswordEmailRequest resetPasswordModel)
        {
            if(resetPasswordModel == null)
            {
                return NotFound();
            }

            MainResponse passwordReset = await _identityService.EmailResetPassword(resetPasswordModel.Email, resetPasswordModel.Password, resetPasswordModel.Token);
            
            if (passwordReset.Success)
            {
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Password has been changed"
                });
            }

            else
            {
                if (passwordReset.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = passwordReset.Errors
                    });
                }
                else
                {
                    return BadRequest(passwordReset.Errors.ToList()[0]);
                }
            }
                
        }


        /// <summary>
        /// User must be logged in.
        /// Token must be in header.
        /// </summary>
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost("PasswordReset")]
        public async Task<IActionResult> PasswordReset(ResetPasswordRequest resetPassword)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
            
            if(accessToken is null)
            {
                return NotFound("Something is wrong with your authorization token");
            }
            if(resetPassword == null)
            {
                return BadRequest("Insert values");
            }


            var reset = await _identityService.PasswordReset(resetPassword.Password, accessToken);

            if (reset.Success)
            {
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Your password has been changed, from now on You can login with different password"
                });
            }
            else
            {
                if (reset.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = reset.Errors
                    });
                }
                else
                {
                    return BadRequest(reset.Errors.ToList()[0]);
                }
            }
        }

        /// <summary>
        /// User must be logged in.
        /// Token must be in header.
        /// </summary>
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost("SendChangeEmailToken")]
        public async Task<IActionResult>ChangeEmailAddressConfirmToken(ChangeEmailAddressRequest newEmail)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
            if(accessToken == null)
            {
                return NotFound("Something is wrong with your authorization token");
            }

            if (newEmail.Email == null)
            {
                return NotFound("Enter valid email");
            }

            ChangeEmailResult changeEmailResult = await _identityService.ChangeEmail(newEmail.Email, accessToken);

            if (changeEmailResult.Success)
            {
                string callback = changeEmailResult.Token;
                Email email = new Email(newEmail.Email, "Change email address confirmation", callback);

                await _emailEmmiter.SendChangeEmailEmail(email); 
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Email change token was send to Your email"
                });
            }
            else
            {
                if (changeEmailResult.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = changeEmailResult.Errors
                    });
                }
                else
                {
                    return BadRequest(changeEmailResult.Errors.ToList()[0]);
                }
            }
                
        }

        /// <summary>
        /// User must be logged in.
        /// Token must be in header.
        /// </summary>
        [Authorize(Policy = "Landlord,Tenant")]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmailAddress(ChangeEmailRequest request) 
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                return NotFound("Something is wrong with your authorization token");
            }

            if (request == null)
            {
                return BadRequest("You must fill in all the fields");
            }

            var changeEmail = await _identityService.ChangeEmailAddress(request.NewEmail, request.Password, request.Token, accessToken);

            if (changeEmail.Success)
            {
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Your email has been changed"
                });
            }
            else
            {
                if (changeEmail.Errors.Count() > 1)
                {
                    return BadRequest(new AuthenticationFailedResponse
                    {
                        Errors = changeEmail.Errors
                    });
                }
                else
                {
                    return BadRequest(changeEmail.Errors.ToList()[0]);
                }
            }

        }

        /// <summary>
        /// This async action verifies if user (e.g. while registering account) can use email of his choice
        /// Accepts email in request body, returns 400 "email not available" / or 200 "email available" message.
        /// </summary>
        /// <param name="checkEmailRequest">Request that containts email address to be verified</param>
        /// <returns></returns>        
        [AllowAnonymous]
        [HttpPost("CheckEmail")]
        public async Task<IActionResult> EmailInUseCheck(CheckEmailRequest checkEmailRequest)
        {
            if (checkEmailRequest.Email == null)
            {
                return NotFound("Enter valid email");
            }

            bool emailInUse = await _identityService.CheckIfEmailInUse(checkEmailRequest.Email);

            if (emailInUse)
            {
                return BadRequest(new SimpleErrorResponse
                {
                    Success = false,
                    Message = "Email not available",
                }); 
            }
            else
            {
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Email available"
                });
            }
        }

        /// <summary>
        /// This async action verifies if user with claim : "tenant.view" exist in DB
        /// Accepts email in request body, returns 400 "Tenant does not exist"/ or 200 "Tenant exists" message.
        /// </summary>
        /// <param name="checkEmailRequest">Request that containts email address to be verified</param>
        /// <returns></returns>        
        [Authorize(Policy = "Landlord")]
        [HttpPost("CheckEmail/Tenant")]
        public async Task<IActionResult> TenantWithSuchMailExistCheck(CheckEmailRequest checkEmailRequest)
        {
            if (checkEmailRequest.Email == null)
            {
                return NotFound("Enter valid email");
            }

            bool tenantExist = await _identityService.CheckIfTenantEmailInUse(checkEmailRequest.Email);

            if (!tenantExist)
            {
                return BadRequest(new SimpleErrorResponse
                {
                    Success = false,
                    Message = "Tenant does not exist",
                });
            }
            else
            {
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Tenant exists"
                });
            }
        }

        /// <summary>
        /// (Authenticated: Landlord ) This async action accepts any userId and removes AspNetUser from Database (CASCADE)
        /// </summary>
        /// <param name="request"> Request contains AspNetUserId (int) to be deleted</param>
        /// <returns></returns>
        [Authorize(Policy = "Landlord")]
        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteUserByAspUserId(DeleteUserRequest request)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                return NotFound("Something is wrong with your authorization token");
            }

            if (request == null)
            {
                return BadRequest("You must fill in all the fields");
            }

            DeleteUserResponse deletionResult = await _identityService.DeleteUserById(request.aspUserId, accessToken);

            if (deletionResult.Success)
            {
                _logger.LogInfo($"Delete user with {request.aspUserId} succeded");
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Confirm, user deleted"
                }); 
            }
            else 
            {
                _logger.LogError("Delete user error occured : " + deletionResult.Errors);
                return BadRequest($"User deletion failed.{deletionResult.Message}");
            }
        }

        /// <summary>
        /// (Unauthenticated) This action acepts only your aspNetUserId, and delete if same id is hidden in token
        /// Action created is for registration cancellacion purposes when user is nor Landlord nor Tenant yet.
        /// </summary>
        /// <param name="request"> Request contains AspNetUserId (int) to be deleted (should be user's own)</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("DeleteSelf")]
        public async Task<IActionResult> DeleteUserConfirmedByToken(DeleteUserRequest request)
        {
            var accessToken = HttpContext.Request.Headers["Authorization"].ToString();
            if (accessToken == null)
            {
                return NotFound("Something is wrong with your authorization token");
            }

            if (request == null)
            {
                return BadRequest("You must fill in all the fields");
            }

            DeleteUserResponse deletionResult = await _identityService.DeleteUserByIdIfTokenHasSameAspNetUserId(request.aspUserId, accessToken);

            if (deletionResult.Success)
            {
                _logger.LogInfo($"Delete user with {request.aspUserId} succeded");
                return Ok(new SimpleSuccessResponse
                {
                    Success = true,
                    Message = "Confirm, user deleted"
                });
            }
            else
            {
                _logger.LogError("Delete user error occured : " + deletionResult.Errors);
                return BadRequest($"User deletion failed.{deletionResult.Message}");
            }
        }

    }
}
