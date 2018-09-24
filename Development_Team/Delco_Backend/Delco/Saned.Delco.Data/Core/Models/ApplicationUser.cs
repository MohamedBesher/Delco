using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Saned.Delco.Data.Core.Enum;

namespace Saned.Delco.Data.Core.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            IsDeleted = false;
            IsSuspend = false;
            IsNotified = false;
            RequestIsSeen = false;
            Abuses = new Collection<Abuse>();
            UserRatings = new Collection<Rating>();
            AgentRatings = new Collection<Rating>();
            Requests = new Collection<Request>();
            DeviceSettings = new Collection<DeviceSetting>();
            RefuseRequests = new Collection<RefuseRequest>();
            ContactUses = new Collection<ContactUs>();


        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string ConfirmedToken { get; set; }
        public string ResetPasswordlToken { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSuspend { get; set; }

        public bool IsOnline{ get; set; } = false;
        public string FullName { get; set; }
       
        public string PhotoUrl { get; set; } 
        public string Address { get; set; }
        public bool IsNotified { get; set; }
        public bool RequestIsSeen { get; set; }
        public string SecondPhoneNumber { get; set; }
        public RequestTypesEnum Type { get; set; }
        //[ForeignKey("City")]

        public long? CityId { get; set; }
        public virtual City City { get; set; }
         public virtual Car Car { get; set; }
        

        #region RelationsClass
        public ICollection<Abuse> Abuses { get; set; }
        public ICollection<Rating> UserRatings { get; set; }
        public ICollection<Rating> AgentRatings { get; set; }
        public ICollection<Request> Requests { get; set; }
        public ICollection<DeviceSetting> DeviceSettings { get; set; }
        public ICollection<RefuseRequest> RefuseRequests { get; set; }
        public ICollection<ContactUs> ContactUses { get; set; }

        #endregion
    }
}
