using McKaren.Admin.Authen;
using McKaren.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace McKaren.Admin.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Auth()
        {
            var ggId = GoogleAuthenticateHelper.GetDefaultGoogleId();
            var authenProvider = new GoogleAuthenticateHelper(ggId.ClientId, ggId.ClientSecret);
            var re = authenProvider.Resolve(new GoogleAuthenticateHelper.GoogleTokenCallbackQuery { 
                Code = Request.QueryString["code"],
                FullUrl = Request.RawUrl
            }, Url.Content("~/Auth", true));
            if (re == null)
            {
                return RedirectToAction("Login", "Account");
            }
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                 1,
                 re.Email,
                 DateTime.Now,
                 DateTime.Now.AddDays(7),
                 false,
                 ""
            );

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);

            return RedirectToAction("Index", "Home");
        }
    }
}