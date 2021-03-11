using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StockControl.Models.Entity;
namespace StockControl.Controllers
{
    public class LoginController : Controller
    {
        stockDatabaseEntities db = new stockDatabaseEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Workers worker, int Remember = 0)
        {

            var query = db.Workers.Where(x => x.WorkerAccountName == worker.WorkerAccountName && x.Password == worker.Password).FirstOrDefault();
            var queryToList = db.Workers.Where(x => x.WorkerAccountName == worker.WorkerAccountName && x.Password == worker.Password).ToList();
            int count = queryToList.Count();
            if (count > 0)
            {
                if (Remember == 1)
                {
                    //giriş bilgilerinizi kullanarak bir uygulama üzerinde oturum açmanıza olanak sağlar
                    //remember tıklandıysa hatırlayacak yoksa hatırkamayacak
                    FormsAuthentication.RedirectFromLoginPage(worker.WorkerAccountName, true);
                }
                else
                {
                    FormsAuthentication.RedirectFromLoginPage(worker.WorkerAccountName, false);
                }
                return RedirectToAction("Index", "Category");
            }
            else
            {
                ViewBag.Mesaj = "Wrong Login!";
                return View();
            }


        }
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        [Obsolete]
        public JsonResult NewAccount(Workers worker)
        {
            var md = FormsAuthentication.HashPasswordForStoringInConfigFile(worker.Password, "md5");
            db.Workers.Add(worker);
            db.SaveChanges();
            return Json("Successfull!");

        }

        [HttpGet]
        [Authorize]
        public ActionResult LoginUpdate(string acount)
        {
            var querym = db.Workers.Where(x => x.WorkerAccountName == User.Identity.Name).FirstOrDefault();
            var queryTolist = db.Workers.Where(x => x.WorkerAccountName == User.Identity.Name).ToList();
            var count = queryTolist.Count();
            return View("LoginUpdate", querym);

        }
        [HttpPost]
        public ActionResult LoginUpdate(Workers account, string newPassword, String newPasswordConfirm)
        {
            var query = db.Workers.Where(x => x.WorkerAccountName == account.WorkerAccountName && x.Password == account.Password).FirstOrDefault();
            var queryT = db.Workers.Where(x => x.WorkerAccountName == account.WorkerAccountName && x.Password == account.Password).ToList();
            var count = queryT.Count();
            if (count > 0)
            {
                if (newPassword!=""&&newPasswordConfirm!="")
                {


                    if (newPassword == newPasswordConfirm)
                    {
                        query.WorkerAccountName = account.WorkerAccountName;
                        query.Password = newPassword;
                        db.SaveChanges();
                        FormsAuthentication.SignOut();
                        return RedirectToAction("Login", "Login");
                    }
                    else
                    {
                        ViewBag.MessageUpdate = "Passwords Are Not The Same!";
                    }
                }
                else
                {
                    ViewBag.MessageUpdate = "Wrong New Password!";
                }
            }
            else
            {
                ViewBag.MessageUpdate = "Wrong Password!";
            }
            return View();

        }

    }
}