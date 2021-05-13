using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var loginModel = new LoginModel()
            {
                UserName = "Admin",
                Password ="",
                Client_Id = "099153c2625149bc8ecb3e85e03f0022",
                Grant_Type = "password"
            };


            return View(loginModel);
        }
    }
}
