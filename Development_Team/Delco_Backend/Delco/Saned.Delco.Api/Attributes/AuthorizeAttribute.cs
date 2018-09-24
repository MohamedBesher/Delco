using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Saned.Delco.Data.Core.Models;
using Saned.Delco.Data.Persistence.Repositories;

namespace Saned.Delco.Api.Attributes
{
   
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]

    public class AuthorizeAttribute : System.Web.Http.AuthorizeAttribute
    {
        public string[] Allowedroles { get; set; }
        public AuthorizeAttribute(params string[] roles)
        {
            this.Allowedroles = roles;
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
        
            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);
            }



        }

        public override  void OnAuthorization(HttpActionContext actionContext)
        {
           AuthRepository repo = new AuthRepository(); 

        

            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                string name = actionContext.RequestContext.Principal.Identity.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    ApplicationUser u =  repo.FindUserByName(name);
                    if (u == null)
                    {
                        actionContext.ModelState.AddModelError("", "User Not found");
                        actionContext.Response =
                            actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,
                                "DeletedUser");
                    }
                    else if (u.IsSuspend.HasValue && u.IsSuspend.Value)
                    {
                        actionContext.ModelState.AddModelError("", "User Has Suspended!");
                        actionContext.Response =
                            actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,
                                "SuspendedUser");
                    }
                    else
                    {
                        if (!IsAuthorized(actionContext))
                        {
                            actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,
                                  "Not authorized");
                            HandleUnauthorizedRequest(actionContext);
                        }

                        //foreach (var role in Allowedroles)
                        //{
                        //    var isInRole = repo.IsInRole(u.Id, role);

                        //    if (!isInRole)
                        //    {
                        //        actionContext.Request.CreateResponse(HttpStatusCode.NonAuthoritativeInformation,
                        //       "Not authorized in this role ");

                        //    }
                        //}
                    }

                }
            
                else
                    base.HandleUnauthorizedRequest(actionContext);

            }
            else
            {
                base.HandleUnauthorizedRequest(actionContext);

            }
        }

       

        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    AuthRepository repo = new AuthRepository();
        //    bool authorize = false;
        //    string name = httpContext.
        //    ApplicationUser u = repo.FindUserByName(name);
        //    foreach (var role in Allowedroles)
        //    {
        //        var isInRole = repo.IsInRole(u.Id, role);

        //        if (isInRole)
        //        {
        //            authorize = true; /* return true if Entity has current user(active) with specific role */
        //        }
        //    }
        //    return authorize;
        //}



    }


}