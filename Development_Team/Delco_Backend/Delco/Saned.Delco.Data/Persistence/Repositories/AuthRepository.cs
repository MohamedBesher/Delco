using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Delco.Data.Core;
using Saned.Delco.Data.Core.Dto;
using Saned.Delco.Data.Core.Enum;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Infrastructure;
using Saned.Delco.Data.Persistence.Tools;
using System.Web.Configuration;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Data.Persistence.Repositories
{
    public enum EmailType
    {
        EmailConfirmation = 1,
        ForgetPassword = 2,
        ContactUs = 3,
        AbuseEmail = 4,
        WelcomeEmail = 5,
        ActivateAgent=6
    }

    public class AuthRepository : IDisposable
    {
        private readonly IUnitOfWorkAsync _unitOfWork;

        private ApplicationDbContext _context;

        private readonly ApplicationUserManagerImpl _userManager;


        public AuthRepository()
        {
            _context = new ApplicationDbContext();
            _userManager = new ApplicationUserManagerImpl();
            _unitOfWork = new UnitOfWorkAsync(_context);
        }

        #region user



        public async Task<IdentityResult> RegisterUser(string smsUrl,
            string name,
            string phoneNumber,
            string email,
            string userName,
            string password,
            string role = "User"
            )
        {
            var user = GetApplicationUser(
                name,
                phoneNumber,
                email,
                userName);




            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                return result;
            else
            {
                await AddRoleToUser(user.Id, role);
                await SendSmsConfirmation(smsUrl, user);
            }

            return result;
        }

        public async Task<IdentityResult> RegisterAgent(
            string name,
            string phoneNumber,
            string email,
            string userName,
            string password,
            string color,
            string companyName,
            string model,
            string plateNumber,
            string type,
            int requestType,
            long cityId,
            int passengerCount,
            string role = "Agent"
            )
        {
            long passengerNumber = 0;
            var firstOrDefault = _context.PassengerNumbers.FirstOrDefault();
            if (firstOrDefault != null)
            {
                 passengerNumber  = firstOrDefault.Id;
            }
            ApplicationUser user = new ApplicationUser
            {
                FullName = name,
                PhoneNumber = phoneNumber,
                Email = email,
                UserName = userName,
                Type = (RequestTypesEnum)requestType,
                CityId = cityId,
                Car =
                    new Car()
                    {
                        Color = color,
                        CompanyName = companyName,
                        PlateNumber = plateNumber,
                        Type = type,
                        Model = model,
                        PassengerNumberId = passengerNumber
                    }
            };


            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return result;
            await AddRoleToUser(user.Id, role);

            //send wait administration approval 


            // await SendEmailConfirmation(user);

            return result;
        }








        public async Task<IdentityResult> CreateUser(
         string name,
         string phoneNumber,
         string email,
         string userName, string password,
         int age,
         string soicalLinks,
         string address,
         string photoUrl
         )
        {

            var user = GetApplicationUser(
                name,
                phoneNumber,
                email,
                userName);


            user.Address = address;
            user.PhotoUrl = photoUrl;
            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user, password);
            return result;
        }

        public async Task<IdentityResult> UpdateUser(string userid,
       string fullName,
       string email
       )
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userid);
            user.FullName = fullName;
            user.Email = email;
            // user.UserName = userName;                    
            return await _userManager.UpdateAsync(user);

        }

        public async Task<IdentityResult> UpdateAgent(string userid,
                                                        string fullName,
                                                        string email,
                                                        string color,
                                                        string companyName,
                                                        string model,
                                                        string plateNumber,
                                                        string type,
                                                        int requestType, long? cityId = null, long? passengerNumberId = null

      )
        {
            ApplicationUser user = await _userManager.Users.Include(x => x.Car).SingleOrDefaultAsync(x => x.Id == userid);
            user.FullName = fullName;
            user.Email = email;

            user.Type = (RequestTypesEnum)requestType;
            user.CityId = cityId;
            if (user.Car != null)
            {
                if (passengerNumberId != null) user.Car.PassengerNumberId = passengerNumberId.Value;
                user.Car.Color = color;
                user.Car.CompanyName = companyName;
                user.Car.Model = model;
                user.Car.PlateNumber = plateNumber;
                user.Car.Type = type;
            }
            var result = await _userManager.UpdateAsync(user);
            return result;
        }


        public async Task<IdentityResult> UpdateAgent(ApplicationUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result;
        }





        public async Task ForgetPassword(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            await SendPasswordResetToken(user);

        }

        public async Task<bool> ForgetPasswordbyPhone(string smsUrl, string phoneNumber)
        {
            var user = await _userManager.FindByPhoneNumberUserManagerAsync(phoneNumber);
            return await SendPasswordResetToken(smsUrl, user);
        }



        public async Task<IdentityResult> ResetPassword(string userId = "", string code = "", string newPassword = "")
        {

            IdentityResult result = await _userManager.ResetPasswordAsync(userId, code, newPassword);
            return result;
        }
        public async Task<ApplicationUser> FindUser(string userName, string password)
        {
            ApplicationUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<List<ApplicationUser>> FindAllUser(int pageNumber, int pageSize, string keyword)
        {
            var user = await _userManager.Users.Where(x =>
               string.IsNullOrEmpty(keyword)
               || x.FullName.ToLower().Contains(keyword.ToLower())
               || x.UserName.ToLower().Contains(keyword.ToLower())
               || x.Email.ToLower().Contains(keyword.ToLower())
               || x.PhoneNumber.ToLower().Contains(keyword.ToLower()))
               .OrderBy(x => x.Id).Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
            return user;
        }

        public async Task<List<ApplicationUser>> FindAllUser(string keyword)
        {
            var user = await _userManager.Users.Where(x =>
               string.IsNullOrEmpty(keyword)
               || x.FullName.ToLower().Contains(keyword.ToLower())
               || x.UserName.ToLower().Contains(keyword.ToLower())
               || x.Email.ToLower().Contains(keyword.ToLower())
               || x.PhoneNumber.ToLower().Contains(keyword.ToLower())).OrderBy(x => x.Id).ToListAsync();
            return user;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var userRole = await IsRoleByName(RolesEnum.User.ToString());
            var users = await FindUsers(userRole.Id);
            return users;
        }

        public IQueryable<ApplicationUser> GetAllAgent()
        {

            var agentsRole = IsRoleByNameWithOutAsync(RolesEnum.Agent.ToString());
            var agents = FindAgents(agentsRole.Id);

            return agents;
        }
        public IQueryable<ApplicationUser> FindAllUsers(string keyword, string inRole)
        {
            string roleId = _context.Roles.FirstOrDefault(u => u.Name == inRole)?.Id;
            var users = _context.Users.Where(m => m.Roles.Any(r => r.RoleId == roleId)); ;
            return users
                .Where(x =>
                    string.IsNullOrEmpty(keyword)
                 // || x.FullName.ToLower().Contains(keyword.ToLower())
                 || x.UserName.ToLower().Contains(keyword.ToLower())
                   // || x.Email.ToLower().Contains(keyword.ToLower())
                   // || x.PhoneNumber.ToLower().Contains(keyword.ToLower())
                   );
        }
        public async Task<List<ApplicationUser>> FindUsers(string roleId)
        {
            var user = await _userManager.Users.Where(x => x.Roles.Any(r => r.RoleId == roleId)).OrderBy(x => x.Id).ToListAsync();

            return user;
        }


        public bool FindUsersInCity(long cityId)
        {
            var user = _userManager.Users.Any(y => y.CityId == cityId);

            return user;
        }

        public async Task<List<ApplicationUser>> FindNewAgentsByRoleId(string roleId)
        {
            var user = await _userManager.Users
                .Where(x => x.Roles.Any(r => r.RoleId == roleId)
                && !x.PhoneNumberConfirmed && x.ConfirmedToken == null)
                .OrderBy(x => x.Id).ToListAsync();

            return user;
        }

        public IQueryable<ApplicationUser> FindAgents(string roleId)
        {
            var user = _userManager.Users.Where(x => x.Roles.Any(r => r.RoleId == roleId)).Include(x => x.Car).OrderBy(x => x.Id);

            return user;
        }

        /// <summary>
        /// update User Info like Name && Phone Number
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="phoneNumber"></param>
        /// <returns>0 if User doesn't exist or 1 if user data updated successfully </returns>
        public int UpdateUser(string userId, string name, string email, int age, string address)
        {

            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return 0;
            }





            user.FullName = name;
            user.Email = email;
            user.Address = address;
            _context.SaveChanges();
            return 1;

        }

        /// <summary>
        /// update user photo
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="imgUrl"></param>
        /// <returns>0 if User doesn't exist or 1 if user data updated successfully</returns>
        public int UpdateUserImage(string userId, string imgUrl)
        {

            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return 0;
            }
            user.PhotoUrl = imgUrl;
            _context.SaveChanges();
            return 1;


        }

        public bool UpdateUserForNotify(string userId, bool isNotify)
        {

            ApplicationUser user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }
            user.IsNotified = isNotify;
            _context.SaveChanges();
            return isNotify;


        }

        public bool CheckifPhoneAvailable(string phoneNumber)
        {

            return _context.Users.Any(u => u.PhoneNumber == phoneNumber);
        }

        public bool CheckifPhoneAvailable(string phoneNumber, string userId)
        {

            return _context.Users.Any(u => u.PhoneNumber == phoneNumber && u.Id != userId);
        }


        public bool CheckifEmailAvailable(string email, string userId)
        {

            return _context.Users.Any(u => u.Email == email && u.Id != userId);
        }

        public bool CheckifEmailAvailable(string email)
        {

            return _context.Users.Any(u => u.Email == email);
        }



        public async Task<bool> IsEmailConfirme(string userId)
        {
            return await _userManager.IsEmailConfirmedAsync(userId);
        }
        public async Task<bool> IsUserArchieve(string userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
            return user.IsDeleted != null && user.IsDeleted.Value;
        }

        public async Task<ApplicationUser> FindUser(string email)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            return user;
        }


        public async Task<ApplicationUser> FindUserbyPhone(string phone)
        {
            ApplicationUser user = await _userManager.FindByPhoneNumberUserManagerAsync(phone);
            return user;
        }


        public ApplicationUser FindUserbyEmail(string email)
        {
            ApplicationUser user = _userManager.FindByEmail(email);
            return user;
        }

        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            bool confirmed = false;
            var result = await _userManager.FindByNameAsync(userId);
            if (result != null)
            {
                if (result.ConfirmedToken == code)
                {
                    result.PhoneNumberConfirmed = true;
                    IdentityResult identity = await this._userManager.UpdateAsync(result);
                    return identity.Succeeded;


                }
                else
                    return false;

            }

            return false;
            //IdentityResult result = await this._userManager.IsPhoneNumberConfirmedAsync(userId);

        }

        public IList<string> GetRoles(string userId)
        {
            IList<string> lst = _userManager.GetRoles(userId);
            return lst;
        }

        private EmailSetting GetEmailMessage(string toName, string code, string messageTemplate)
        {
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(messageTemplate);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@FullName", toName);
            emailSettings.MessageBodyAr = emailSettings.MessageBodyAr.Replace("@code", "Code=" + code);

            return emailSettings;
        }
        private ApplicationUser GetApplicationUser(string name, string phone, string email, string userName)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                PhoneNumber = phone,
                Email = email,
                FullName = name,
            };
            return user;
        }

        public async Task<IdentityResult> AddRoleToUser(string userId, string role)
        {
            var result = await _userManager.AddToRoleAsync(userId, role);
            return result;
        }

        public async Task<bool> SendSmsConfirmation(string url, ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(EmailType.EmailConfirmation.GetHashCode().ToString());
            EmailManager email = new EmailManager();
            string message = emailSettings.MessageBodyAr.Replace("@code", code);
            return email.SendSMS(url, user.PhoneNumber, message);

        }
        public async Task<bool> ReSendSmsConfirmation(string url, ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(EmailType.EmailConfirmation.GetHashCode().ToString());
            EmailManager email = new EmailManager();
            string message = emailSettings.MessageBodyAr.Replace("@code", user.UserName);
            return email.SendSMS(url, user.PhoneNumber, message);

        }
        private async Task SendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        public async Task ReSendEmailConfirmation(ApplicationUser user)
        {

            var code = await GenerateToken(user.Id, EmailType.EmailConfirmation);
            await SendEmail(user, code, EmailType.EmailConfirmation.GetHashCode().ToString());
        }
        private async Task SendPasswordResetToken(ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.ForgetPassword);


            await SendEmail(user, code, EmailType.ForgetPassword.GetHashCode().ToString());


        }

        private async Task<bool> SendPasswordResetToken(string smsUrl, ApplicationUser user)
        {
            var code = await GenerateToken(user.Id, EmailType.ForgetPassword);
            EmailSetting emailSettings = _unitOfWork.EmailSetting.GetEmailSetting(EmailType.ForgetPassword.GetHashCode().ToString());
            EmailManager email = new EmailManager();
            string message = emailSettings.MessageBodyAr.Replace("@code", code);
            //message = message.Replace("@code", code);
            return email.SendSMS(smsUrl, user.PhoneNumber, message);

        }

        public async Task<string> GenerateToken(string userId, EmailType emailType)
        {
            switch (emailType)
            {
                case EmailType.EmailConfirmation:
                   // return await _userManager.GenerateEmailConfirmationTokenAsync(userId);
                    return await GenerateTokenNumber(userId);

                case EmailType.ForgetPassword:
                    //return await _userManager.GeneratePasswordResetTokenAsync(userId);
                    return await GenerateResetTokenNumber(userId);

                default:
                    return "";
            }

        }
        public async Task<string> GenerateTokenNumber(string userId)
        {

           var user =await _userManager.FindByIdAsync(userId);
            var generateRandomCode = GenerateRandomCode();
            user.ConfirmedToken = generateRandomCode;
            await _userManager.UpdateAsync(user);
            return generateRandomCode;

        }

        public async Task<string> GenerateResetTokenNumber(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);
            var userResetPasswordlToken = GenerateRandomCode();
            user.ResetPasswordlToken = userResetPasswordlToken;
            await _userManager.UpdateAsync(user);
            return userResetPasswordlToken;

        }

        private static string GenerateRandomCode()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                GenerateRandomCode();
            }
            return r;
        }


        public async Task SendEmail(ApplicationUser user, string code, string messageTemplate)
        {
            EmailSetting emailMessage = GetEmailMessage(user.UserName, code, messageTemplate);
            await _userManager.SendEmailAsync(user.Id, messageTemplate, emailMessage.MessageBodyAr);
        }
        public async Task<IdentityResult> ChangePassword(string userId, string oldPassword, string newPassword)
        {
            IdentityResult result = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            return result;
        }


        #endregion




        #region token

        public Client FindClient(string clientId)
        {
            var client = _context.Clients.Find(clientId);

            return client;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {

            var existingToken =
                _context.RefreshTokens
                    .FirstOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);

            if (existingToken != null)
            {
                var result = await RemoveRefreshToken(existingToken);
            }

            _context.RefreshTokens.Add(token);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            if (refreshToken != null)
            {
                _context.RefreshTokens.Remove(refreshToken);
                return await _context.SaveChangesAsync() > 0;
            }

            return false;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);

            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.ToList();
        }

        #endregion

        #region Soical
        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            ApplicationUser user = await _userManager.FindAsync(loginInfo);

            return user;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                await AddRoleToUser(user.Id, "User");
            }

            return result;
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            var result = await _userManager.AddLoginAsync(userId, login);

            return result;
        }
        #endregion

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this._context == null)
            {
                return;
            }

            this._context.Dispose();
            this._userManager.Dispose();
            this._context = null;
            this._unitOfWork.Dispose();
        }

        public async Task<ApplicationUser> FindUserByUserName(string userName)
        {
            ApplicationUser user = await _userManager.Users.Include(x => x.Car).Include(x => x.Roles).SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }
        public async Task<ApplicationUser> FindAgentByUserName(string userName)
        {
            ApplicationUser user = await _userManager.Users.Include(u => u.Car)
                                     .SingleOrDefaultAsync(x => x.UserName == userName);
            return user;
        }
        public ApplicationUser CheckUserNameExist(string userName)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.UserName == userName);
            return user;
        }
        public async Task<ApplicationUser> FindUserByUserId(string id)
        {
            ApplicationUser user = await _userManager.Users.Include(x => x.Car).Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == id);
            return user;
        }

        public async Task<IdentityResult> EditImage(string picture, string id)
        {
            ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == id);
            user.PhotoUrl = picture;
            return await _userManager.UpdateAsync(user);
        }

        public bool IsInRole(string id, string role)
        {
            return _userManager.IsInRole(id, role);
        }

        public async Task<ApplicationUser> FindUserById(string userId)
        {
            ApplicationUser user = await _userManager.Users.Include(x => x.Car).SingleOrDefaultAsync(x => x.Id == userId);
            return user;
        }



        public async Task<IdentityResult> ValidateAsync(string email, string phoneNumber, string userId = "")
        {
            List<string> errors = new List<string>();

            if (!string.IsNullOrEmpty(email))
            {
                var findAsyncByEmail = await FindAsyncByEmail(email, userId);
                if (findAsyncByEmail != null)
                {
                    string errorMsg = "Email '" + email + "' is already related to User.";
                    errors.Add(errorMsg);
                }

            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                var findAsyncByEmail = await FindAsyncByPhone(phoneNumber, userId);
                if (findAsyncByEmail != null)
                {
                    string errorMsg = "PhoneNumber '" + phoneNumber + "' is already related to User.";
                    errors.Add(errorMsg);
                }
            }




            return errors.Any()
                ? IdentityResult.Failed(errors.ToArray())
                : IdentityResult.Success;
        }

        private async Task<ApplicationUser> FindAsyncByPhone(string phoneNumber, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ApplicationUser user = await _userManager.Users.
                    SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
                return user;
            }
            else
            {
                ApplicationUser user = await _userManager.Users.
                    SingleOrDefaultAsync(x => x.PhoneNumber == phoneNumber && x.Id != userId);
                return user;
            }

        }

        private async Task<ApplicationUser> FindAsyncByEmail(string email, string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == email);
                return user;
            }
            else
            {
                ApplicationUser user = await _userManager.Users.
                    SingleOrDefaultAsync(x => x.Email == email && x.Id != userId);
                return user;
            }
        }


        public async Task DeleteById(string userId)
        {
            using (_userManager)
            {
                ApplicationUser user = await _userManager.Users.SingleOrDefaultAsync(x => x.Id == userId);
                if (user != null)
                {
                    IdentityResult result = _userManager.Delete(user);
                }
            }


        }

        public async Task<int> IsRoleExit(string role)
        {
            var item = await _context.Roles.FirstOrDefaultAsync(z => z.Name == role);
            return item != null ? 1 : 0;
        }
        public async Task<IdentityRole> IsRoleByName(string role)
        {
            var item = await _context.Roles.FirstOrDefaultAsync(z => z.Name == role);
            return item;
        }

        public IdentityRole IsRoleByNameWithOutAsync(string role)
        {
            var item = _context.Roles.FirstOrDefault(z => z.Name == role);
            return item;
        }

        public async Task<IdentityResult> RemoveFromRole(string userId, string role)
        {
            return await _userManager.RemoveFromRoleAsync(userId, role);
        }
        public async Task<IdentityResult> RemoveFromRoles(string userId, string[] role)
        {
            return await _userManager.RemoveFromRolesAsync(userId, role);
        }
        public async Task<IList<string>> GetUserRoles(string userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }
        public List<IdentityRole> GetRoles()
        {
            return _context.Roles.Select(x => x).ToList();
        }


        public async Task<IList<ApplicationUser>> GetDuplicatedUsers(string email, string phoneNumber)
        {
            return await _userManager.Users.Where(x => x.Email == email || x.UserName == phoneNumber).ToListAsync();
        }

        public async Task<IdentityResult> SuspendAgent(ApplicationUser user)
        {
            user.IsSuspend = true;
            user.IsOnline = false;
            return await _userManager.UpdateAsync(user);
        }

        public ApplicationUser FindUserByName(string name)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.UserName == name);
            return user;
        }


        public ApplicationUser FindUserByIdNotAsync(string id)
        {
            ApplicationUser user = _userManager.Users.SingleOrDefault(x => x.Id == id);
            return user;
        }



        public async Task<IdentityResult> UpdateStatus(ApplicationUser user, bool status)
        {
            var oldUser = await _userManager.FindByIdAsync(user.Id);
            oldUser.IsOnline = true;

            return await _userManager.UpdateAsync(oldUser);

        }
        public async Task<IdentityResult> UpdatePhoneNumberConfirmed(ApplicationUser user, bool status)
        {
            user.PhoneNumberConfirmed = true;
            return await _userManager.UpdateAsync(user);

        }

        


        public async Task<IdentityResult> UpdateIsSeen(ApplicationUser user, bool status)
        {

            user.RequestIsSeen = status;
            return await _userManager.UpdateAsync(user);

        }

        public async Task<IdentityResult> AddPhone(ApplicationUser user, string phone)
        {

            user.SecondPhoneNumber = phone;
            return await _userManager.UpdateAsync(user);

        }

        public async Task<bool> ChangePhone(ApplicationUser user, string phone, string code)
        {
            string firstphone = "";
            if (user.ConfirmedToken == code)
            {
                firstphone = user.PhoneNumber;
                user.PhoneNumber = user.SecondPhoneNumber;
                user.SecondPhoneNumber = firstphone;
                await _userManager.UpdateAsync(user);
                return true;
            }
            return false;



        }
    }
}