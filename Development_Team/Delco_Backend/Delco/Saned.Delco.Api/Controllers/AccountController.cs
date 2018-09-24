using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saned.Delco.Api.Models;
using Saned.Delco.Api.Properties;
using Saned.Delco.Api.Results;
using Saned.Delco.Api.Validators;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;
using Saned.Delco.Data.Persistence.Tools;
using Saned.Jawla.Api.ViewModels;

namespace Saned.Delco.Api.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : BaseController
    {
        private IAuthenticationManager Authentication => Request.GetOwinContext().Authentication;


        [AllowAnonymous]
        [Route("RegisterUser")]
        public async Task<IHttpActionResult> RegisterUser(UserViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                string smsUrl= Settings.Default.SmsUrl.ToString();
                var result =
                    await _repo.RegisterUser(smsUrl, viewModel.FullName,
                        viewModel.PhoneNumber,
                        viewModel.Email,
                        viewModel.UserName,
                        viewModel.Password
                    );

                var errorResult = GetErrorResult(result);

                if (errorResult != null)
                    return errorResult;

                var u = await _repo.FindUser(viewModel.UserName, viewModel.Password);
                //if(u!=null && !string.IsNullOrEmpty(u.Id))
                //    SendSms(viewModel.UserName,)

                return Ok(u.Id);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                ErrorSaver.SaveError(ex.Message);
                return BadRequest("Register " + msg);
            }
        }


        [AllowAnonymous]
        [Route("RegisterAgent")]
        public async Task<IHttpActionResult> RegisterAgent(AgentViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);




                var result =
                    await _repo.RegisterAgent(viewModel.FullName,
                        viewModel.PhoneNumber,
                        viewModel.Email,
                        viewModel.UserName,
                        viewModel.Password,
                        viewModel.Color,
                        viewModel.CompanyName,
                        viewModel.Model,
                        viewModel.PlateNumber,
                        viewModel.Type,
                        viewModel.RequestType,
                        viewModel.CityId,
                        viewModel.PassengerCount
                    );

                var errorResult = GetErrorResult(result);

                if (errorResult != null)
                    return errorResult;

                var u = await _repo.FindUser(viewModel.UserName, viewModel.Password);


                var EmailManager = new EmailManager();

                var setting =
                    _UnitOfWork
                        .EmailSetting
                        .GetEmailSetting(EmailType.WelcomeEmail.GetHashCode().ToString());


                EmailManager.SendWelcomeEmail(EmailType.WelcomeEmail.GetHashCode().ToString(),
                    u.Email,
                    setting.MessageBodyAr.Replace("@FullName", u.FullName));

                return Ok(u.Id);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return BadRequest("Register " + msg);
            }
        }


        [AllowAnonymous]
        [Route("SendEmail")]
        public async Task<IHttpActionResult> SendEmail()
        {
            var str = "";
            try
            {
                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);
                var mngMail = new EmailManager();
                str = mngMail.SendActivationEmail(EmailType.EmailConfirmation.GetHashCode().ToString(), u.Email, "ok");
                await _repo.SendEmail(u, "Test For Send", EmailType.EmailConfirmation.GetHashCode().ToString());
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return BadRequest("SendEmail --- " + msg);
            }
            return Ok(str);
        }

        [AllowAnonymous]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(ConfirmViewModel confirmViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _repo.ConfirmEmail(confirmViewModel.UserId, confirmViewModel.Code);                
                if (result)
                  return Ok();
                else
                {
                    ModelState.AddModelError("", "Invalid token");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
              
                var msg = ex.Message;
                ErrorSaver.SaveError(msg);
                return BadRequest("ConfirmEmail --- " + msg);
            }
        }

        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordViewModel resetPasswordView)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = await _repo.FindUserbyPhone(resetPasswordView.Phone);
                if (user == null)
                {
                    ModelState.AddModelError("", "Not Found");
                    return BadRequest(ModelState);
                }
                var result = await _repo.ResetPassword(user.Id, resetPasswordView.Code, resetPasswordView.Password);
                var errorResult = GetErrorResult(result);

                if (errorResult != null)
                    return errorResult;
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                ErrorSaver.SaveError(msg);
                return BadRequest("ResetPassword --- " + msg);
            }
        }

        [AllowAnonymous]
        [Route("ForgetPassword")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgetPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            try
            {
                if (!ModelState.IsValid || forgotPasswordViewModel == null)
                    return BadRequest(ModelState);

                var user = await _repo.FindUserbyPhone(forgotPasswordViewModel.Phone);
                if (user == null)
                {
                    ModelState.AddModelError("", "Not Found");
                    return BadRequest(ModelState);
                }
                string smsUrl = Settings.Default.SmsUrl.ToString();
                await _repo.ForgetPasswordbyPhone(smsUrl, forgotPasswordViewModel.Phone);
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                ErrorSaver.SaveError(msg);

                return BadRequest("ForgetPassword --- " + msg);
            }
        }


        [Attributes.Authorize(Roles = "Agent")]
        [Route("ChangeAgentInfo")]
        [HttpPost]
        public async Task<IHttpActionResult> AgentChangeInfo(AgentProfileViewModel viewProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);
                ;
                if (u == null)
                {
                    ModelState.AddModelError("", "You Need To Login");
                    return BadRequest(ModelState);
                }

                var result = await _repo.UpdateAgent(
                    u.Id,
                    viewProfile.FullName,
                    viewProfile.Email,
                    viewProfile.Color,
                    viewProfile.CompanyName,
                    viewProfile.Model,
                    viewProfile.PlateNumber,
                    viewProfile.Type,
                    viewProfile.RequestType
                );
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error);
                    return BadRequest(ModelState);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }


        [Route("UserInfo")]
        [Attributes.Authorize(Roles = "User,Agent")]

        [HttpPost]
        public async Task<IHttpActionResult> GetUserInfoById(UserInfoViewModel model)
        {
            var user = await _repo.FindUserByUserId(model.UserId);

            if (user == null)
                return NotFound();

            var roles = await _repo.GetUserRoles(model.UserId);

            var agentStatisticsModel = new AgentStatisticsModel();
            if (roles.FirstOrDefault() == RolesEnum.Agent.ToString())
                agentStatisticsModel = await GetUserStatistic(model.UserId);


            return Ok(new UserEditViewmodel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Type = (int?)user.Type,
                CityId = user.CityId,
                PhotoUrl = user.PhotoUrl,
                Address = user.Address,
                CompanyName = user.Car != null ? user.Car.CompanyName : string.Empty,
                CarType = user.Car != null ? user.Car.Type : string.Empty,
                Model = user.Car != null ? user.Car.Model : string.Empty,
                Color = user.Car != null ? user.Car.Color : string.Empty,
                PlateNumber = user.Car != null ? user.Car.PlateNumber : string.Empty,
                PassengerNumberId = user.Car?.PassengerNumberId,
                IsSuspend = user.IsSuspend,
                Role = roles != null ? roles.First() : string.Empty,
                AgentStatisticsModel = agentStatisticsModel
            });
        }

        [Route("NotifyUsers")]
        [Attributes.Authorize(Roles = "Agent , User")]
        public async Task<IHttpActionResult> NotifyUsers(UserNotifyViewModel model)
        {
            try
            {
                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);
                if (u == null)
                    return BadRequest("You Need To Login");
                var result = _repo.UpdateUserForNotify(u.Id, model.IsNotify);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        #region Attributes

        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeImage")]
        [HttpPost]

        #endregion

        public async Task<IHttpActionResult> ChangeImage(UserPhotoViewModel viewProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);
                if (u == null)
                {
                    ModelState.AddModelError("", "You Need To Login");
                    return BadRequest(ModelState);
                }
                var imagePath = "none";
                if (viewProfile.Picture.ToLower() == "none")
                {
                    var result = _repo.UpdateUserImage(u.Id, null);
                }
                else
                {
                    imagePath = ImageSaver.SaveImage(viewProfile.Picture);
                    var result = _repo.UpdateUserImage(u.Id, imagePath);
                }


                return Ok(imagePath);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        #region  Admin

        [Route("GetUsers")]
        [HttpPost]
        public async Task<IHttpActionResult> GetUsers(PagingViewModel model)
        {
            var result =
                await _repo.FindAllUser(model.PageNumber, model.PageSize, model.Keyword);
            var totalCount = (await _repo.FindAllUser(model.Keyword)).Count;
            var pagedSet = new PaginationSet<ApplicationUser>
            {
                Items = result,
                Page = model.PageNumber,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((decimal)totalCount / model.PageNumber)
            };

            return Ok(pagedSet);
        }


        [Route("Users/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(string id)
        {
            var result = await _repo.FindUserById(id);

            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                _repo.DeleteById(id);
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                return BadRequest("Register " + msg);
            }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _repo.Dispose();

            base.Dispose(disposing);
        }

        [Route("ChangePassword")]
        [Attributes.Authorize(Roles = "Agent,User")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordViewModel changePasswordViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);

                var result = await _repo.ChangePassword(u.Id, changePasswordViewModel.OldPassword,
                    changePasswordViewModel.NewPassword);

                var errorResult = GetErrorResult(result);

                if (errorResult != null)
                    return errorResult;
                return Ok();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException.Message;
                return BadRequest("ChangePassword --- " + msg);
            }
        }

        [Route("GetUserInfo")]
        [Attributes.Authorize(Roles = "Agent,User")]

        [HttpPost]
        public async Task<IHttpActionResult> GetUserInfo(DeviceSettingModel model)
        {
            try
            {
                var userName = User.Identity.GetUserName();
                var user = await GetApplicationUser(userName);
                if (user == null)
                    return NotFound();


                var roles = await _repo.GetUserRoles(user.Id);
                var agentStatisticsModel = new AgentStatisticsModel();
                if (roles.FirstOrDefault() == RolesEnum.Agent.ToString())
                    agentStatisticsModel = await GetUserStatistic(user.Id);

                model.ApplicationUserId = user.Id;


                var device =
                    _UnitOfWork.DeviceSettingRepository.All()
                        .Any(u => u.ApplicationUserId == model.ApplicationUserId && u.DeviceId == model.DeviceId);

                if (!device)
                {
                    var deviceModel = Mapper.Map<DeviceSettingModel, DeviceSetting>(model);
                    _UnitOfWork.DeviceSettingRepository.Create(deviceModel);
                    _UnitOfWork.Commit();
                }

                return Ok(new UserEditViewmodel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Type = (int?)user.Type,
                    CityId = user.CityId,
                    PhotoUrl = user.PhotoUrl,
                    Address = user.Address,
                    CompanyName = user.Car != null ? user.Car.CompanyName : string.Empty,
                    CarType = user.Car != null ? user.Car.Type : string.Empty,
                    Model = user.Car != null ? user.Car.Model : string.Empty,
                    Color = user.Car != null ? user.Car.Color : string.Empty,
                    PlateNumber = user.Car != null ? user.Car.PlateNumber : string.Empty,
                    PassengerNumberId = user.Car?.PassengerNumberId,
                    IsSuspend = user.IsSuspend,
                    IsNotified = user.IsNotified,
                    Role = roles != null ? roles.First() : string.Empty,
                    AgentStatisticsModel = agentStatisticsModel
                });
            }
            catch (Exception e)
            {
                ErrorSaver.SaveError(e.Message);
                return BadRequest(e.Message);
            }
        }


        [Attributes.Authorize(Roles = "User")]
        [Route("ChangeUserInfo")]
        [HttpPost]
        public async Task<IHttpActionResult> UserChangeInfo(UserEditInfoViewmodel viewProfile)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userName = User.Identity.GetUserName();
                var u = await GetApplicationUser(userName);


                var result = await _repo.UpdateUser(u.Id, viewProfile.FullName, viewProfile.Email);
                if (!result.Succeeded)
                    return GetErrorResult(result);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("GetAgentInfo")]
        [Attributes.Authorize(Roles = "Agent")]
        [HttpPost]
        public async Task<IHttpActionResult> GetAgentInfo()
        {
            var userName = User.Identity.GetUserName();
            var user = await GetApplicationUser(userName);


            if (user != null)
                return Ok(user);
            return NotFound();
        }


        [Attributes.Authorize(Roles = "User,Agent")]
        [Route("ChangeStatus/{status}")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangeStatus(bool status)
        {
            try
            {
                var userId = GetUserId();
                var user = await GetApplicationUserById(userId);
                await _repo.UpdateStatus(user, status);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("ChangeStatus --- " + ex.Message);
            }
        }




        [Attributes.Authorize(Roles = "User,Agent")]
        [Route("AddPhone/{secondNumber}")]
        [HttpPost]
        public async Task<IHttpActionResult> AddPhone(string secondNumber)
        {
            try
            {
                var userId = GetUserId();
                var user = await GetApplicationUserById(userId);
                await _repo.AddPhone(user, secondNumber);

                string smsUrl = Settings.Default.SmsUrl.ToString();

                var sent = await _repo.SendSmsConfirmation(smsUrl, user);


                return Ok();
            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex.Message);
                return BadRequest("ChangeStatus --- " + ex.Message);
            }
        }






        [Attributes.Authorize(Roles = "User,Agent")]
        [Route("ConfirmPhone")]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePhone(PhoneViewModel phone)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = GetUserId();
                var user = await GetApplicationUserById(userId);

                var result=await _repo.ChangePhone(user:user, phone:phone.Phone,code:phone.Code);
                if (result)
                    return Ok();
                else
                {
                    ModelState.AddModelError("Invalid token", "Invalid token");
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                ErrorSaver.SaveError(ex.Message);
                return BadRequest("ChangeStatus --- " + ex.Message);
            }
        }




        #region Soical

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            var redirectUri = string.Empty;

            if (error != null)
                return BadRequest(Uri.EscapeDataString(error));

            if (!User.Identity.IsAuthenticated)
                return new ChallengeResult(provider, this);

            var redirectUriValidationResult = ValidateClientAndRedirectUri(Request, ref redirectUri);

            if (!string.IsNullOrWhiteSpace(redirectUriValidationResult))
                return BadRequest(redirectUriValidationResult);

            var externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
                return InternalServerError();

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            IdentityUser user =
                await _repo.FindAsync(new UserLoginInfo(externalLogin.LoginProvider, externalLogin.ProviderKey));

            var hasRegistered = user != null;

            redirectUri =
                string.Format("{0}#external_access_token={1}&provider={2}&haslocalaccount={3}&external_user_name={4}",
                    redirectUri,
                    externalLogin.ExternalAccessToken,
                    externalLogin.LoginProvider,
                    hasRegistered,
                    externalLogin.UserName);

            return Redirect(redirectUri);
        }

        [AllowAnonymous]
        [Route("ReSendConfirmationCode/{id}")]
        public async Task<IHttpActionResult> ReSendConfirmationCode(string id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest("User Id Not Send");

                var user = await _repo.FindUserByUserName(id);
                if (user == null)
                    return BadRequest("هذا المستخدم غير موجود.");
                string smsUrl = Settings.Default.SmsUrl;
                if (user.PhoneNumberConfirmed)
                    return Ok("تم التفعيل من قبل.");
                await _repo.ReSendSmsConfirmation(smsUrl, user);
               
                return Ok("تم ارسال كود التفعيل بنجاح.");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                ErrorSaver.SaveError(msg);
                return BadRequest("ReSendConfirmationCode " + msg);
            }
        }

        // POST api/Account/RegisterExternal
        [AllowAnonymous]
        [Route("RegisterExternal2")]
        public async Task<IHttpActionResult> RegisterExternalbck(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);
            if (verifiedAccessToken == null)
                return BadRequest("Invalid Provider or External Access Token");

            var user = await _repo.FindAsync(new UserLoginInfo(model.Provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (hasRegistered)
                return BadRequest("External user is already registered");

            user = new ApplicationUser { UserName = model.UserName };

            var result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
                return GetErrorResult(result);

            var info = new ExternalLoginInfo
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, verifiedAccessToken.user_id)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
                return GetErrorResult(result);

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(model.UserName);

            return Ok(accessTokenResponse);
        }


        [AllowAnonymous]
        [Route("RegisterExternal3")]
        public async Task<IHttpActionResult> RegisterExternal2(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ApplicationUser user;
            if (model.Provider != "Twitter")
            {
                var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);

                if (verifiedAccessToken == null)
                    return BadRequest("Invalid Provider or External Access Token");
                model.UserId = verifiedAccessToken.user_id;
                user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            }
            else
            {
                user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            }


            var hasRegistered = user != null;

            if (hasRegistered)
            {
                // return BadRequest("External user is already registered");
                var token = GenerateLocalAccessTokenResponseUpdate(user);
                return Ok(token);
            }

            if (string.IsNullOrWhiteSpace(model.Name))
                model.Name = model.Provider + "User";
            user = new ApplicationUser
            {
                UserName = model.UserName,
                FullName = model.Name
            };

            var result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
                return GetErrorResult(result);

            var info = new ExternalLoginInfo
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, model.UserId)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
                return GetErrorResult(result);

            //generate access token response
            user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            var accessTokenResponse = GenerateLocalAccessTokenResponseUpdate(user);

            return Ok(accessTokenResponse);
        }

        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            ApplicationUser user;
            //if (model.Provider != "Twitter")
            //{
            //    var verifiedAccessToken = await VerifyExternalAccessToken(model.Provider, model.ExternalAccessToken);

            //    if (verifiedAccessToken == null)
            //    {
            //        return BadRequest("Invalid Provider or External Access Token"+model.ExternalAccessToken +"provider"+ model.Provider );
            //    }
            //    model.UserId = verifiedAccessToken.user_id;
            //    user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            //}
            //else
            {
                user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            }


            var hasRegistered = user != null;

            if (hasRegistered)
            {
                // return BadRequest("External user is already registered");
                var token = GenerateLocalAccessTokenResponseUpdate(user);
                return Ok(token);
            }

            if (string.IsNullOrWhiteSpace(model.Name))
                model.Name = model.Provider + "User";
            user = new ApplicationUser
            {
                UserName = model.UserName,
                FullName = model.Name
            };

            var result = await _repo.CreateAsync(user);
            if (!result.Succeeded)
                return GetErrorResult(result);

            var info = new ExternalLoginInfo
            {
                DefaultUserName = model.UserName,
                Login = new UserLoginInfo(model.Provider, model.UserId)
            };

            result = await _repo.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
                return GetErrorResult(result);

            //generate access token response
            user = await _repo.FindAsync(new UserLoginInfo(model.Provider, model.UserId));
            var accessTokenResponse = GenerateLocalAccessTokenResponseUpdate(user);

            return Ok(accessTokenResponse);
        }

        private JObject GenerateLocalAccessTokenResponseUpdate(ApplicationUser user)
        {
            var tokenExpiration = TimeSpan.FromDays(1);

            var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim("sub", user.UserName));
            using (var repo = new AuthRepository())
            {
                var userRoles = repo.GetRoles(user.Id);
                foreach (var role in userRoles)
                    identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            var props = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            var tokenResponse = new JObject(
                new JProperty("userName", user.UserName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                //new JProperty("PhotoUrl", user.PhotoUrl ?? string.Empty),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            );

            return tokenResponse;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ObtainLocalAccessToken")]
        public async Task<IHttpActionResult> ObtainLocalAccessToken(string provider, string externalAccessToken)
        {
            if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(externalAccessToken))
                return BadRequest("Provider or external access token is not sent");

            var verifiedAccessToken = await VerifyExternalAccessToken(provider, externalAccessToken);
            if (verifiedAccessToken == null)
                return BadRequest("Invalid Provider or External Access Token");

            IdentityUser user = await _repo.FindAsync(new UserLoginInfo(provider, verifiedAccessToken.user_id));

            var hasRegistered = user != null;

            if (!hasRegistered)
                return BadRequest("External user is not registered");

            //generate access token response
            var accessTokenResponse = GenerateLocalAccessTokenResponse(user.UserName);

            return Ok(accessTokenResponse);
        }

        #endregion

        #region Helpers

        private string ValidateClientAndRedirectUri(HttpRequestMessage request, ref string redirectUriOutput)
        {
            Uri redirectUri;

            var redirectUriString = GetQueryString(Request, "redirect_uri");

            if (string.IsNullOrWhiteSpace(redirectUriString))
                return "redirect_uri is required";

            var validUri = Uri.TryCreate(redirectUriString, UriKind.Absolute, out redirectUri);

            if (!validUri)
                return "redirect_uri is invalid";

            var clientId = GetQueryString(Request, "client_id");

            if (string.IsNullOrWhiteSpace(clientId))
                return "client_Id is required";

            var client = _repo.FindClient(clientId);

            if (client == null)
                return string.Format("Client_id '{0}' is not registered in the system.", clientId);

            if (
                !string.Equals(client.AllowedOrigin, redirectUri.GetLeftPart(UriPartial.Authority),
                    StringComparison.OrdinalIgnoreCase))
                return string.Format("The given URL is not allowed by Client_id '{0}' configuration.", clientId);

            redirectUriOutput = redirectUri.AbsoluteUri;

            return string.Empty;
        }

        private string GetQueryString(HttpRequestMessage request, string key)
        {
            var queryStrings = request.GetQueryNameValuePairs();

            if (queryStrings == null) return null;

            var match = queryStrings.FirstOrDefault(keyValue => string.Compare(keyValue.Key, key, true) == 0);

            if (string.IsNullOrEmpty(match.Value)) return null;

            return match.Value;
        }

        private async Task<ParsedExternalAccessToken> VerifyExternalAccessToken(string provider, string accessToken)
        {
            ParsedExternalAccessToken parsedToken = null;

            var verifyTokenEndPoint = "";

            if (provider == "Facebook")
            {
                //You can get it from here: https://developers.facebook.com/tools/accesstoken/
                //More about debug_tokn here: http://stackoverflow.com/questions/16641083/how-does-one-get-the-app-access-token-for-debug-token-inspection-on-facebook
                var appToken = "276971536031476|lPmkKAidu0_m9fY3DnOTsVs5lyw";
                verifyTokenEndPoint =
                    string.Format("https://graph.facebook.com/debug_token?input_token={0}&access_token={1}", accessToken,
                        appToken);
            }
            else if (provider == "Google")
            {
                verifyTokenEndPoint = string.Format("https://www.googleapis.com/oauth2/v1/tokeninfo?access_token={0}",
                    accessToken);
            }
            else
            {
                return null;
            }

            var client = new HttpClient();
            var uri = new Uri(verifyTokenEndPoint);
            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                dynamic jObj = (JObject)JsonConvert.DeserializeObject(content);

                parsedToken = new ParsedExternalAccessToken();

                if (provider == "Facebook")
                {
                    parsedToken.user_id = jObj["data"]["user_id"];
                    parsedToken.app_id = jObj["data"]["app_id"];

                    if (
                        !string.Equals(Startup.FacebookAuthOptions.AppId, parsedToken.app_id,
                            StringComparison.OrdinalIgnoreCase))
                        return null;
                }
                else if (provider == "Google")
                {
                    parsedToken.user_id = jObj["user_id"];
                    parsedToken.app_id = jObj["audience"];

                    if (
                        !string.Equals(Startup.GoogleAuthOptions.ClientId, parsedToken.app_id,
                            StringComparison.OrdinalIgnoreCase))
                        return null;
                }
            }

            return parsedToken;
        }

        private JObject GenerateLocalAccessTokenResponse(string userName)
        {
            var tokenExpiration = TimeSpan.FromDays(1);

            var identity = new ClaimsIdentity(OAuthDefaults.AuthenticationType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.Add(tokenExpiration)
            };

            var ticket = new AuthenticationTicket(identity, props);

            var accessToken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);

            var tokenResponse = new JObject(
                new JProperty("userName", userName),
                new JProperty("access_token", accessToken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpiration.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString())
            );

            return tokenResponse;
        }

        private async Task<AgentStatisticsModel> GetUserStatistic(string agentId)
        {
            double ratevalue = 0;
            var rating = await _UnitOfWork.RatingRepository.Filter(x => x.AgentId == agentId).ToListAsync();
            if (rating != null && rating.Count > 0)
                ratevalue = Math.Round(rating.Average(x => x.Degree), 1);

            var requestCount =
                _UnitOfWork.RequestRepository.Filter(
                    x => x.Status == RequestStatusEnum.Delivered && x.AgentId == agentId).Count();

            var totalPrice =
                _UnitOfWork.RequestRepository.Filter(
                    x => x.Status == RequestStatusEnum.Delivered && x.AgentId == agentId).ToList().Sum(x => x.Price);
            decimal managementPercentage = 0;

            var setting = _UnitOfWork.SettingRepository.All().FirstOrDefault();
            if (setting != null)
                managementPercentage = totalPrice * setting.ManagementPercentage / 100;


            var agentProfit = totalPrice - managementPercentage;

            return new AgentStatisticsModel
            {
                AgentRate = ratevalue,
                RequestCount = requestCount,
                DelivaryPrice = totalPrice,
                ManagPercentage = managementPercentage,
                AgentProfit = agentProfit
            };
        }

        #endregion
    }
}