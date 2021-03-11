using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockControl.Models.Entity;

//burada paging kütüphanesini tanımlayacağız
using PagedList;
using PagedList.Mvc;
namespace StockControl.Controllers
{
    public class CustomerController : Controller
    {
        stockDatabaseEntities db = new stockDatabaseEntities();
        // GET: Customer
        [Authorize]
        public ActionResult Index()
        {
            //var query = db.Customers.Where(x => x.CUSTOMERSTATUS == true).ToList().ToPagedList(number,6);
            var query = db.Customers.Where(x => x.CUSTOMERSTATUS == true).ToList();
            return View(query);
        }


        //1
        [HttpGet]
        public ActionResult NewCustomer()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        public ActionResult NewCustomer(Customers customer)
        {
            if (!ModelState.IsValid)
            {
                return View("NewCustomer");
            }
            customer.CUSTOMERSTATUS = true;
            db.Customers.Add(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //2
        public ActionResult Delete(int id)
        {
            //But-rası veri siliyor biz veritabanından silinmesin istiyoruz
            /*var query = db.Customers.Find(id);
            db.Customers.Remove(query);
            db.SaveChanges();*/
            var query = db.Customers.Find(id);
            query.CUSTOMERSTATUS = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //3
        [HttpGet]
        [Authorize]
        public ActionResult Update(int id)
        {
            var query = db.Customers.Find(id);
            return View("Update",query);
        }
        [HttpPost]
        public ActionResult Update(Customers customer)
        {
            var query = db.Customers.Find(customer.CUSTOMERID);
            query.CUSTOMERNAME = customer.CUSTOMERNAME;
            query.CUSTOMERSURNAME = customer.CUSTOMERSURNAME;
            query.CUSTOMERSTATUS = customer.CUSTOMERSTATUS;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}