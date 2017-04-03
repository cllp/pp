﻿using PP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace PP.Api.Attributes
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        //public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        //{
        //    if (Authorize(actionContext))
        //    {
        //        return;
        //    }
        //    HandleUnauthorizedRequest(actionContext);
        //}

        protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            //Custom logic here

            return base.IsAuthorized(actionContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

            CoreFactory.Log.Warning("UnauthorizedRequest. Name: " + principal.Identity.Name + ". IsAuthenticated: " + principal.Identity.IsAuthenticated.ToString());

            throw new UnauthorizedAccessException("User is not authorized to access this resource");
        }

        private bool Authorize(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            try
            {
                var principal = actionContext.ControllerContext.RequestContext.Principal as ClaimsPrincipal;

                //boolean logic to determine if you are authorized.  
                //We check for a valid token in the request header or cookie.
                //return true;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}