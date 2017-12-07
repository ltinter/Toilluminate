using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;

namespace ToilluminateModel
{
   public class RequestAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //get ticket from httpcontext
            var userticket = actionContext.Request.Headers.GetCookies().Select(a => a["userticket"]).FirstOrDefault().Value;
            if ((userticket != null) && (userticket != ""))
            {
                ///validate user info from ticket
                if (ValidateUserInfo(userticket))
                {
                    base.IsAuthorized(actionContext);
                }
                else
                {
                    HandleUnauthorizedRequest(actionContext);
                }
            }
            //return 401 without ticket
            else
            {
                var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                if (isAnonymous) base.OnAuthorization(actionContext);
                else HandleUnauthorizedRequest(actionContext);
            }
        }

        //validate user info from ticket
        private bool ValidateUserInfo(string encryptTicket)
        {
            //Decrypt Ticket
            var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;

            //get username from ticket
            var index = strTicket.IndexOf("&");
            string userName = strTicket.Substring(0, index);

            if(HttpContext.Current.Session[userName].Equals(encryptTicket))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}