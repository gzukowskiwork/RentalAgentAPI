using Entities;
using Entities.Models;
using Identity.Domain;
using Identity.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using System.Web;

namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RepositoryContext _repositoryContext;

        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, RepositoryContext repositoryContext)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _repositoryContext = repositoryContext;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { $"User with this email already exists" }
                };
            }


            ApplicationUser newUser = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            IdentityResult createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddClaimAsync(newUser, new Claim("landlord.view", "true"));
            return await GenarateAuthenticationResult(newUser);
        }


        public async Task<AuthenticationResult> TenantRegisterAsync(string email, string password)
        {
            ApplicationUser existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { $"User with this email already exists" }
                };
            }

            ApplicationUser newUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FirstLogin = true
            };

            IdentityResult createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            await _userManager.AddClaimAsync(newUser, new Claim("tenant.view", "true"));
            return await GenarateAuthenticationResult(newUser);
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

           
            
            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { $"User with this email does not exist" }
                };
            }

            bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            
            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Incorrect Password" }
                };
            }

            return await GenarateAuthenticationResult(user);
        }


        private async Task<AuthenticationResult> GenarateAuthenticationResult(ApplicationUser newUser)
        {
            int? landlordId = -1, tenantId = -1;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

           


            CheckUser(newUser, ref landlordId, ref tenantId);

            List<Claim> claims = await CreateClaim(newUser, landlordId, tenantId);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials
                                        (new SymmetricSecurityKey(key),
                                        SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Id = newUser.Id,
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        private async Task<List<Claim>> CreateClaim(ApplicationUser newUser, int? landlordId, int? tenantId)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Aud, "SimpleFakeRent"),
                new Claim("firstLogin", newUser.FirstLogin.ToString(), ClaimValueTypes.Boolean)
            };

            claims.Add(new Claim("userId", newUser.Id.ToString(), ClaimValueTypes.Integer));

            if (landlordId != -1)
            {
                claims.Add(new Claim("id", landlordId.ToString(), ClaimValueTypes.Integer));
                claims.Add(new Claim("userType", "landlord", ClaimValueTypes.String));
            }
            else if (tenantId != -1)
            {
                claims.Add(new Claim("id", tenantId.ToString(), ClaimValueTypes.Integer));
                claims.Add(new Claim("userType", "tenant", ClaimValueTypes.String));
            }

            var userClaims = await _userManager.GetClaimsAsync(newUser);
            claims.AddRange(userClaims);
            return claims;
        }

        private void CheckUser(ApplicationUser newUser, ref int? landlordId, ref int? tenantId)
        {
            if (_repositoryContext.Tenants.Any(x => x.AspNetUsersId.Equals(newUser.Id)) != false)
            {
                tenantId = _repositoryContext.Tenants.FirstOrDefault(x => x.AspNetUsersId.Equals(newUser.Id)).Id;
            }
            if (_repositoryContext.Landlords.Any(x => x.AspNetUsersId.Equals(newUser.Id)) != false)
            {
                landlordId = _repositoryContext.Landlords.FirstOrDefault(x => x.AspNetUsersId.Equals(newUser.Id)).Id;
            }
        }

        public async Task<ResetPasswordResult> ForgotPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ResetPasswordResult
                {
                    Errors = new[] { "User not found" }
                };
            }

            var reetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            return new ResetPasswordResult
            {
                Email = user.Email,
                Success = true,
                Token = reetToken
            };
        }


        public async Task<MainResponse> EmailResetPassword(string email, string password, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new MainResponse
                {
                    Errors = new[] { "User not found" }
                };
            }

            PasswordValidator<ApplicationUser> passwordValidator = new PasswordValidator<ApplicationUser>();
            IdentityResult result = await passwordValidator.ValidateAsync(_userManager, null, password);
            if (result.Succeeded)
            {
                IdentityResult passwordResetResponse = await _userManager.ResetPasswordAsync(user, token, password);
               
                if (passwordResetResponse.Succeeded)
                {
                    return new MainResponse
                    {
                        Success = true
                    };
                } 
                else
                {
                    return new MainResponse
                    {
                        Errors = new[] { "Password was not reset. Token used or expired." }
                    };
                }

                
            }
            else
            {
                return new MainResponse
                {
                    Errors = new[] { "Incorect password, it must include at least one number(0-9), at least 8 characters, capital and small letters, and at least one special characters(!@#$%...) " }
                };
            }
        }



        public async Task<MainResponse> PasswordReset(string password, string token)
        {

            token = token.Substring(7);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

            string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;

            if (email == null)
            {
                return new MainResponse
                {
                    Errors = new[] { "Email not found" }
                };
            }

            ApplicationUser user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return new MainResponse
                {
                    Errors = new[] { "User not found" }
                };
            }
            user.FirstLogin = false;
            IdentityResult updateFirstLogin = await _userManager.UpdateAsync(user);
            if (!updateFirstLogin.Succeeded)
            {
                return new MainResponse
                {
                    Errors = new[] { "Update not succeeded" }
                };
            }
            string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            PasswordValidator<ApplicationUser> passwordValidator = new PasswordValidator<ApplicationUser>();
            IdentityResult result = await passwordValidator.ValidateAsync(_userManager, null, password);

            if(result.Succeeded)
            {
                await _userManager.ResetPasswordAsync(user, resetToken, password);
                return new MainResponse
                {
                    Success = true
                };
            }
            else
            {
                return new MainResponse
                {
                    Errors= new [] {"Incorect password, it must include at least one number(0-9), at least 8 characters, capital and small letters, and at least one special characters(!@#$%...) "}
                };
            }
        }



        public async Task<ChangeEmailResult> ChangeEmail(string newEmail, string token)
        {
            token = token.Substring(7);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

            string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;

            if (email == null)
            {
                return new ChangeEmailResult
                {
                    Errors = new[] { "User not found" }
                };
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ChangeEmailResult
                {
                    Errors = new[] { "User not found" }
                };
            }

            string changeEmailToken = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);

            return new ChangeEmailResult
            {
                Success = true,
                Token = changeEmailToken,
                Email = newEmail
            };
        }

        public async Task<ChangeEmailSuccessResponse> ChangeEmailAddress(string newEmail, string password, string resetToken, string jwtToken)
        {
            jwtToken = jwtToken.Substring(7);

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(jwtToken);

            string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ChangeEmailSuccessResponse
                {
                    Errors = new[] { "User not found" }
                };
            }

            bool userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (userHasValidPassword)
            {
                IdentityResult setNewEmail = await _userManager.ChangeEmailAsync(user, newEmail, resetToken);

                if (setNewEmail.Succeeded)
                {
                    IdentityResult setNewUserName = await _userManager.SetUserNameAsync(user, newEmail);

                    if (setNewUserName.Succeeded)
                    {
                        return new ChangeEmailSuccessResponse
                        {
                            Success = true,
                            Email = newEmail
                        };
                    }
                    else
                    {
                        return new ChangeEmailSuccessResponse
                        {
                            Errors = setNewUserName.Errors.Select(x => x.Description)
                        };
                    }

                    
                }

                else
                {
                    return new ChangeEmailSuccessResponse
                    {
                        Errors = setNewEmail.Errors.Select(x => x.Description)
                    };
                }
                

            }

            return new ChangeEmailSuccessResponse
            {
                Errors = new[] { "Something went wrong" }
            };
        }

        public async Task<bool> CheckIfEmailInUse(string email)
        {
            ApplicationUser userFound = await _userManager.FindByEmailAsync(email);

            if (userFound != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Checks by e-mail address if such tenant user exists in DB.
        /// </summary>
        /// <param name="email">user email address as string</param>
        /// <returns>boolean: exist or does not</returns>
        public async Task<bool> CheckIfTenantEmailInUse(string email)
        {
            ApplicationUser userFound = await _userManager.FindByEmailAsync(email);
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(userFound);

            if (userFound == null || userClaims == null)
            {
                return false;
            }

            foreach (Claim claim in userClaims)
            {
                if (claim.Type == "tenant.view" && claim.Value == "true")
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes a User permanently, CASCADE.
        /// </summary>
        /// <param name="id"> Id of a AspnetUser to be deleted, that comes with a request</param>
        /// <param name="jwtToken"> Token of a user requesting delete action: Landlord or Tenant</param>
        /// <returns></returns>
        public async Task<DeleteUserResponse> DeleteUserById(int id, string jwtToken)
        {
            jwtToken = jwtToken.Substring(7);

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(jwtToken);

            string aspNetUserId = jsonToken.Claims.First(claim => claim.Type == "userId").Value;

            ApplicationUser userToBeDeleted = await _userManager.FindByIdAsync(id.ToString());

            if ( userToBeDeleted == null)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Errors = new[] { "User not found" },
                    Message = "User does not exist"
                };
            } 

            IdentityResult deleteAspNetUser = await _userManager.DeleteAsync(userToBeDeleted);

            if (deleteAspNetUser.Errors.Any())
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Errors = new[] { "Something happend while erasing user" },
                    Message = "User not deleted"
                };
            }
            return new DeleteUserResponse
            {
                Success = true,
                Message = "User deleted"
            };
        }

        /// <summary>
        /// Deletes a User permanently, CASCADE.
        /// </summary>
        /// <param name="id"> Id of a AspnetUser to be deleted, that comes with a request</param>
        /// <param name="jwtToken"> Token of a user requesting delete action: Landlord or Tenant</param>
        /// <returns></returns>
        public async Task<DeleteUserResponse> DeleteUserByIdIfTokenHasSameAspNetUserId(int id, string jwtToken)
        {
            jwtToken = jwtToken.Substring(7);

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(jwtToken);

            int aspNetUserId = Int32.Parse(jsonToken.Claims.First(claim => claim.Type == "userId").Value);
            
            // check if requested id is the same as in the token - proves that sender deletes himself
            if ( aspNetUserId != id)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Errors = new[] { "You request to delete different user than yourself" },
                    Message = "You cannot delete different user"
                };
            }

            ApplicationUser userToBeDeleted = await _userManager.FindByIdAsync(id.ToString());

            if (userToBeDeleted == null)
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Errors = new[] { "User not found" },
                    Message = "User does not exist"
                };
            }

            IdentityResult deleteAspNetUser = await _userManager.DeleteAsync(userToBeDeleted);

            if (deleteAspNetUser.Errors.Any())
            {
                return new DeleteUserResponse
                {
                    Success = false,
                    Errors = new[] { "Something happend while erasing user" },
                    Message = "User not deleted"
                };
            }
            return new DeleteUserResponse
            {
                Success = true,
                Message = "User deleted"
            };
        }

        /// <summary>
        /// This method checks if ApplicationUser exists with given Id (int)
        /// Returns boolean positive if exist and negative if not.
        /// </summary>
        /// <param name="id">AspNetUserId</param>
        /// <returns></returns>
        public async Task<bool> CheckIfAspNetUserExistByAspNetUserId(int id)
        {
            ApplicationUser userFound = await _userManager.FindByIdAsync(id.ToString());

            if(userFound == null)
            {
                return false;
            } 
            else
            {
                return true;
            }
            
        }

        /// <summary>
        /// Reads email from received Requester's token and finds AspUser by this email, then anonimizes it be replacing user name and email with auto generated email
        /// </summary>
        /// <param name="token"> Requesting person's token</param>
        /// <returns></returns>
        public async Task<AnonimizationResponse> AnonimizeUserByEmailFromToken(string token)
        {
            // Open token
            token = token.Substring(7);

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jsonToken = handler.ReadJwtToken(token);

            string email = jsonToken.Claims.First(claim => claim.Type == "email").Value;

            // Generate new fake mail address
            string emailGenerated = "";
            bool emailIsAvailable = false;
            while (!emailIsAvailable)
            {
                emailGenerated = GenerateNewMail();
                ApplicationUser emailCheck = await _userManager.FindByEmailAsync(emailGenerated);
                emailIsAvailable = emailCheck == null ? true : false;
            }

            // Find User to Anonimize
            ApplicationUser userToAnonimize = await _userManager.FindByEmailAsync(email);
            if (userToAnonimize == null)
            {
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = new[] { "User not found" },
                    Message = "User with this email was not found."
                };
            }

            // Result of email anonimization
            IdentityResult EmailAnonimizationResult = await _userManager.SetEmailAsync(userToAnonimize, emailGenerated);
            if (EmailAnonimizationResult.Errors.Any())
            {
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = EmailAnonimizationResult.Errors.Select(x => x.Description),
                    Message = "Could not anonimize user email, see errors."
                };
            }
            // Result of change user name
            IdentityResult UserNameAnonimizationResult = await _userManager.SetUserNameAsync(userToAnonimize, emailGenerated.ToLower());
            if (UserNameAnonimizationResult.Errors.Any())
            {
                //Console.WriteLine("------> Złapałem błąd po anonimizacji!");
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = UserNameAnonimizationResult.Errors.Select(x => x.Description),
                    Message = "Could not anonimize user name, see errors."
                };
            }

            return new AnonimizationResponse
            {
                Success = true,
                Message = "User successfully anonimized"
            };
        }

        public async Task<AnonimizationResponse> AnonimizeUserByUserEmail(string email)
        {
            // Generate new fake mail address
            string emailGenerated = "";
            bool emailIsAvailable = false;
            while (!emailIsAvailable)
            {
                emailGenerated = GenerateNewMail();
                ApplicationUser emailCheck = await _userManager.FindByEmailAsync(emailGenerated);
                emailIsAvailable = emailCheck == null ? true : false;
            }

            // Find User to Anonimize
            ApplicationUser userToAnonimize = await _userManager.FindByEmailAsync(email);
            if (userToAnonimize == null)
            {
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = new[] { "User not found" },
                    Message = "User with this email was not found."
                };
            }

            // Result of email anonimization
            IdentityResult EmailAnonimizationResult = await _userManager.SetEmailAsync(userToAnonimize, emailGenerated);
            if (EmailAnonimizationResult.Errors.Any())
            {
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = EmailAnonimizationResult.Errors.Select(x => x.Description),
                    Message = "Could not anonimize user email, see errors."
                };
            }
            // Result of change user name
            IdentityResult UserNameAnonimizationResult = await _userManager.SetUserNameAsync(userToAnonimize, emailGenerated.ToLower());
            if (UserNameAnonimizationResult.Errors.Any())
            {
                //Console.WriteLine("------> Złapałem błąd po anonimizacji!");
                return new AnonimizationResponse
                {
                    Success = false,
                    Errors = UserNameAnonimizationResult.Errors.Select(x => x.Description),
                    Message = "Could not anonimize user name, see errors."
                };
            }

            return new AnonimizationResponse
            {
                Success = true,
                Message = "User successfully anonimized"
            };
        }

        public string GenerateNewMail()
        {
            StringBuilder fakeEmailGenerated = new StringBuilder();
            Random random = new Random();
            int emailLength = random.Next(7, 20);
            char singleLetter;

            for (int i = 0; i < emailLength; i++)
            {
                double generatedfloatNumber = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * generatedfloatNumber));
                singleLetter = Convert.ToChar(shift + 65);
                fakeEmailGenerated.Append(singleLetter);
            }
            fakeEmailGenerated.Append("@rodo.pl");

            return fakeEmailGenerated.ToString();
        }
        /// <summary>
        /// Get AspNetUser by aspnNetUserId
        /// </summary>
        /// <param name="id"> AspNetUser = ApplicationUser.Id (of type: integer)</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetApplicationUserById(int id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }

        /// <summary>
        /// Get AspNetUser by aspNetUserEmail
        /// </summary>
        /// <param name="id"> AspNetUser = ApplicationUser.Email (of type: string)</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetApplicationUserByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        /// <summary>
        /// Get AspNetUser who is claimed to be Tenant by aspNetUserEmail
        /// </summary>
        /// <param name="id"> AspNetUser = ApplicationUser.Email (of type: string)</param>
        /// <returns></returns>
        public async Task<ApplicationUser> GetApplicationUserTenantByEmail(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            IList<Claim> userClaims = await _userManager.GetClaimsAsync(user);

            if (user == null || userClaims == null)
            {
                return null;
            }

            foreach (Claim claim in userClaims)
            {
                if (claim.Type == "tenant.view" && claim.Value == "true")
                {
                    return user;
                }
            }

            return null;
        }

    }
}
