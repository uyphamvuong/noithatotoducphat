using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Products
        [Auth]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.GroupProduct);
            return View(products.ToList());
        }

        // GET: Admin/Products/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,SeoUrl,ImgUrl,Order,Price,IsDisable,IsKM,GroupProductId,Intro,Keyword,Des")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.SeoUrl == null) { product.SeoUrl = DreamCMS.FuncHelp.DHelp.SEOurl(product.ProductName); }
                if (db.Products.Where(x => x.SeoUrl == product.SeoUrl).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên sản phẩm đã được sử dụng!!!";
                    ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName", product.GroupProductId);
                    return View(product);
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName", product.GroupProductId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName", product.GroupProductId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,SeoUrl,ImgUrl,Order,Price,IsDisable,IsKM,GroupProductId,Intro,Keyword,Des")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.SeoUrl == null) { product.SeoUrl = DreamCMS.FuncHelp.DHelp.SEOurl(product.ProductName); }
                if (db.Products.Where(x => x.SeoUrl == product.SeoUrl && x.ProductId!=product.ProductId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên sản phẩm đã được sử dụng!!!";
                    ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName", product.GroupProductId);
                    return View(product);
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GroupProductId = new SelectList(db.GroupProducts, "GroupProductId", "GroupName", product.GroupProductId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
