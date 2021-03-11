using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StockControl.Models.Entity;

namespace StockControl.Controllers
{
    public class SalesController : Controller
    {

        stockDatabaseEntities db = new stockDatabaseEntities();
        [Authorize]
        public ActionResult Index()
        {
            //var query = db.Sales.ToList().ToPagedList(number, 6);
            var query = db.Sales.ToList();
            return View(query);
        }

        [HttpGet]
        [Authorize]
        public ActionResult NewSales()
        {
            return View();

        }
        [HttpPost]
        public ActionResult NewSales(Sales sale)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Mesaj = "Wrong ADD!";
                return View();

            }
            db.Sales.Add(sale);
            db.SaveChanges();
            return View("Index");
        }
    }
}