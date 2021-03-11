using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockControl.Models.Entity;
//tanımlama yapıyorux pagedlist için
using PagedList;
using PagedList.Mvc;
namespace StockControl.Controllers
{
    public class CategoryController : Controller
    {   // GET: Category
        //db database table ları tutuyor
        stockDatabaseEntities db = new stockDatabaseEntities();
       [Authorize]
        public ActionResult Index()
        {
            //toPageList 1den başla 6 kayıt getir
            //var query = db.Categories.Where(x => x.CATEGORYSTATUS == true).ToList().ToPagedList(number, 6);
            //categorydeki tüm verileri getiren bir select sorgusu
            var query = db.Categories.Where(x => x.CATEGORYSTATUS == true).ToList();
            return View(query);
        }
        //1
        //Sayfa yüklendiğinde sadece sayfayı dönder
        [HttpGet]
        [Authorize]
        public ActionResult NewCategory()
        {
            return View();
        }
        //ben butona basana kadar yani post yapana kadar ekleme yapma.
        [HttpPost]
        public ActionResult NewCategory(Categories category)
        {
            //eğer modelin durumunda is valid yani doğrulama işlemi yapılmadıysa 
            if (!ModelState.IsValid)
            {
                return View("NewCategory");
            }
            category.CATEGORYSTATUS = true;
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index"); 
        }


        //2
        public ActionResult Delete(int id)
        {
            //But-rası veri siliyor biz veritabanından silinmesin istiyoruz
            /*var query = db.Categories.Find(id);
            db.Categories.Remove(query);
            db.SaveChanges();*/
            var query = db.Categories.Find(id);
            query.CATEGORYSTATUS = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //3
        [HttpGet]
        [Authorize]
        public ActionResult Update(int id)
        {
            var query = db.Categories.Find(id);
            return View("Update", query);

        }
        [HttpPost]
        public ActionResult Update(Categories category)
        {
            var query = db.Categories.Find(category.CATEGORYID);
            query.CATEGORYNAME = category.CATEGORYNAME;
            query.CATEGORYSTATUS = category.CATEGORYSTATUS;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}