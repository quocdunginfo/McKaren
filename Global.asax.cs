﻿using McKaren.Core;
using McKaren.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace McKaren
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        //let us take out the username now                
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        using (var db = new McKarenDb())
                        {
                            var user = db.Users.FirstOrDefault(x => x.UserName.ToLower().Equals(username.ToLower()));
                            if (user != null)
                            {
                                CustomPrincipal newUser = new CustomPrincipal(user.UserName);
                                newUser.roles = user.Roles.Select(x => x.Id).ToList();
                                System.Web.HttpContext.Current.User = newUser;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //went wrong
                    }
                }
            }
        }
    }
}
