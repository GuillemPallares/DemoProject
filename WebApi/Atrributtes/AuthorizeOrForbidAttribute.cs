using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApi.Atrributtes
{
    /// <summary>
    /// Custom AuthorizationAttribue
    /// </summary>
    /// <remarks>
    /// If the user is authenticated will assign a 403 response, else will assign a 401.
    /// </remarks>
    public class AuthorizeOrForbidAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Handles Unathorized requests.
        /// </summary>
        /// <param name="actionContext">Http ActionContext</param>
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, actionContext.RequestContext.Principal.Identity.Name + " does not have permissions for this request.");
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Authorization has been denied for this request.");
            }
        }
    }
}