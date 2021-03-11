using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockControl.Models.Entity;

using PagedList;
using PagedList.Mvc;
using System.IO;

namespace StockControl.Controllers
{
    public class ProductController : Controller
    {
        stockDatabaseEntities db = new stockDatabaseEntities();
        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
            //var query = db.Products.Where(x => x.PRODUCTSTATUS == true).ToList().ToPagedList(number, 6);
            var query = db.Products.Where(x => x.PRODUCTSTATUS == true).ToList();
            return View(query);
        }



        [HttpGet]
        [Authorize]
        public ActionResult NewProduct()
        {
            // sayfa yüklendiğindeCategory tabledan kategory name i alıyoruz ve valuya ıdsini yazacak..
            List<SelectListItem> querym = (from i in db.Categories.ToList()
                                          select new SelectListItem
                                          {
                                              Text = i.CATEGORYNAME,
                                              Value = i.CATEGORYID.ToString()
                                          }).ToList();
            //Vievbag sorgudan gelen değerleri diğer sayfaya taşıyor...
            ViewBag.query = querym;
            return View();
        }
        [HttpPost]
        public ActionResult NewProduct(Products product)
        {
            if (Request.Files.Count>0)
            {
                string classname = Path.GetFileName(Request.Files[0].FileName);
                string extension = Path.GetExtension(Request.Files[0].FileName);
                string way = "~/Image/" + classname;
                Request.Files[0].SaveAs(Server.MapPath(way));
                product.PRODUCTIMAGE="/Image/"+ classname;
            }
            //Drop DownListte içinde seçilen değeri bize FirstorDefault ile getirir..
            var queryDropDownList = db.Categories.Where(x => x.CATEGORYID == product.Categories.CATEGORYID).FirstOrDefault();
            product.Categories = queryDropDownList;
            product.PRODUCTSTATUS = true;
            db.Products.Add(product);
            db.SaveChanges();
            //Bunla işlemler bittikten sonra listelemenin olduğu sayfaya yönlendiriliyor...
            return RedirectToAction("Index");
        }



        public ActionResult Delete(int id)
        {
            //But-rası veri siliyor biz veritabanından silinmesin istiyoruz
            /*var query = db.Products.Find(id);
            db.Products.Remove(query);
            db.SaveChanges();*/
            var query = db.Products.Find(id);
            query.PRODUCTSTATUS = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Update(int id)
        {
            var query = db.Products.Find(id);
            return View("Update", query);

        }
        [HttpPost]
        public ActionResult Update(Products product)
        {
            if (Request.Files.Count > 0)
            {
                string classname = Path.GetFileName(Request.Files[0].FileName);
                string extension = Path.GetExtension(Request.Files[0].FileName);
                string way = "~/Image/" + classname;
                Request.Files[0].SaveAs(Server.MapPath(way));
                product.PRODUCTIMAGE = "/Image/".ToString() + classname;
            }
            var query = db.Products.Find(product.PRODUCTID);          
            query.PRODUCTNAME = product.PRODUCTNAME;
            query.BRAND = product.BRAND;
            query.PRICE = product.PRICE;
            query.STOCK = product.STOCK;
            query.PRODUCTIMAGE = product.PRODUCTIMAGE;
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        [Authorize]
        public ActionResult ProductDetail(int id)
        {
            
            var query = db.Products.Where(x => x.PRODUCTID == id).ToList();
            int count = query.Count();
            if (count>0)
            {
                return View(query);
            }
            else
            {
                return RedirectToAction("Index");
            }
            

        }
       
    }
}