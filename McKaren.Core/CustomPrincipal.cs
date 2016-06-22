using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace McKaren.Core
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Equals(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }

        public int UserId { get; set; }
        public string FullName { get; set; }
        public List<string> roles { get; set; }
    }
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permissions { get; set; }

        protected virtual CustomPrincipal CurrentUser
        {
            get { return System.Web.HttpContext.Current.User as CustomPrincipal; }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                return;
            }
            throw new InvalidOperationException("Unauthorized");
        }
    }
}
