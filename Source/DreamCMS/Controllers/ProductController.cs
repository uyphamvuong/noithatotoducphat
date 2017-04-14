using DreamCMS.Models;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DreamCMS.Controllers
{
    public class ProductController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: GroupProduct
        public ActionResult Index(int? page)
        {
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            List<Product> ListProduct = db.Products.Include(x => x.GroupProduct).Where(x => x.IsDisable == false).OrderByDescending(x => x.Order).ToList();
            if (ListProduct == null) { ListProduct = new List<Product>(); }
            return View(ListProduct.ToPagedList(currentPageIndex, 12));
        }

        //ViewGroup
        public ActionResult ViewGroup(string titleidgroup, int? page)
        {
            GroupProduct gp = db.GroupProducts.Where(x => x.SeoUrl == titleidgroup && x.IsDisable == false).FirstOrDefault();
            if (gp == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }

            ViewBag.GroupProduct = gp;
            int currentPageIndex = page.HasValue ? page.Value - 1 : 0;

            List<Product> ListProduct = db.Products.Where(x => x.IsDisable == false && x.GroupProductId == gp.GroupProductId).OrderByDescending(x => x.Order).ToList();
            if (ListProduct == null) { ListProduct = new List<Product>(); }
            return View(ListProduct.ToPagedList(currentPageIndex, 12));
        }

        //ViewDetail
        public ActionResult ViewDetail(string titleidgroup, string titleid)
        {
            Product pr = db.Products.Include(x => x.GroupProduct).Where(x => x.SeoUrl == titleid && x.IsDisable == false).FirstOrDefault();
            if (pr == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }

            List<Product> ListProduct = db.Products.Include(x => x.GroupProduct).Where(x => x.IsDisable == false && x.ProductId != pr.ProductId && x.GroupProductId == pr.GroupProductId).OrderByDescending(x => x.ProductId).OrderByDescending(x => x.Order).Take(8).ToList();
            if (ListProduct == null) { ListProduct = new List<Product>(); }
            ViewBag.ListProduct = ListProduct;
            return View(pr);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Cart(int? Id)
        {

            if (Session["sp1"] == null)
            {
                Session["sp1"] = Id;
                ViewBag.sp1 = db.Products.Where(p => p.ProductId == Id).FirstOrDefault();
            }
            else
            {
                int idsp1 = Convert.ToInt16(Session["sp1"]);
                ViewBag.sp1 = db.Products.Where(p => p.ProductId == idsp1).FirstOrDefault();
                if (idsp1 != Id)
                {
                    if (Session["sp2"] == null)
                    {
                        Session["sp2"] = Id;
                        ViewBag.sp2 = db.Products.Where(p => p.ProductId == Id).FirstOrDefault();
                    }
                    else
                    {
                        int idsp2 = Convert.ToInt16(Session["sp2"]);
                        ViewBag.sp2 = db.Products.Where(p => p.ProductId == idsp2).FirstOrDefault();
                    }
                }

            }
            return View();
        }
    }
}