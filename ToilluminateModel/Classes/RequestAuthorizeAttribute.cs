﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
            if (SkipAuthorization(actionContext)) return;
            //get ticket from httpcontext
            var userticket = actionContext.Request.Headers.GetCookies().Select(a => a["userticket"]).FirstOrDefault().Value;
            if ((userticket != null) && (userticket != "") && PublicMethods.ValidateUserInfo(userticket) != "")
            {
                base.IsAuthorized(actionContext);
            }
            //return 401 without ticket or session timeout
            else
            {
                //var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                //bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                //if (isAnonymous) base.OnAuthorization(actionContext);
                //else
                HandleUnauthorizedRequest(actionContext);
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            Contract.Assert(actionContext != null);

            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                       || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}