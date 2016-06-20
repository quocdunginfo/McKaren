using McKaren.Admin.Authen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace McKaren.Admin.Controllers
{
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
            return RedirectToAction("Index", "Home");
        }
    }
}