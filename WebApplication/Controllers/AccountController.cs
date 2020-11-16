using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //ad authentication
        public IActionResult Login()
        {
            string loginName = HttpContext.User.Identity.Name;
            string ip = ADHelper.GetIP4Address(HttpContext.Connection.RemoteIpAddress.ToString());
            var uis = ADHelper.GetUser(loginName.Split("\\")[0], "samaccountname", loginName.Split("\\")[1]);
            return Content("");
        }
    }
}