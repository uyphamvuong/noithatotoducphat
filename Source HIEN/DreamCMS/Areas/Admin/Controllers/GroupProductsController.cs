using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class GroupProductsController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/GroupProducts
        [Auth]
        public ActionResult Index()
        {
            var groupProducts = db.GroupProducts.Include(g => g.GroupProductParrent);
            return View(groupProducts.ToList());
        }

        // GET: Admin/GroupProducts/Create
        [Auth]
        public ActionResult Create()
        {
            List<GroupProduct> ListGroupProduct = db.GroupProducts.Where(x => x.CountNode < 2).OrderBy(x=>x.GroupName).ToList();
            ListGroupProduct.Insert(0,new GroupProduct() { GroupName = "Gốc" });
            ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName");
            return View();
        }

        // POST: Admin/GroupProducts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "GroupProductId,GroupName,SeoUrl,Order,IsDisable,ImgUrl,IdRoot,CountNode")] GroupProduct groupProduct)
        {
            List<GroupProduct> ListGroupProduct = db.GroupProducts.Where(x => x.CountNode < 2).OrderBy(x => x.GroupName).ToList();
            ListGroupProduct.Insert(0, new GroupProduct() { GroupName = "Gốc" });

            if (ModelState.IsValid)
            {
                if (groupProduct.SeoUrl == null) { groupProduct.SeoUrl = DreamCMS.FuncHelp.DHelp.SEOurl(groupProduct.GroupName); }
                if (db.GroupProducts.Where(x => x.SeoUrl == groupProduct.SeoUrl).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên nhóm đã được sử dụng!!!";
                    ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName", groupProduct.IdRoot);
                    return View(groupProduct);
                }

                if (groupProduct.IdRoot == 0) { groupProduct.IdRoot = null; groupProduct.CountNode = 0; }
                else
                {
                    GroupProduct gpRoot = db.GroupProducts.Where(x => x.GroupProductId == groupProduct.IdRoot).FirstOrDefault();
                    groupProduct.CountNode = gpRoot.CountNode + 1;
                }

                db.GroupProducts.Add(groupProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName", groupProduct.IdRoot);
            return View(groupProduct);
        }

        // GET: Admin/GroupProducts/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            if (groupProduct == null)
            {
                return HttpNotFound();
            }
            List<GroupProduct> ListGroupProduct = db.GroupProducts.Where(x => x.CountNode < 2).OrderBy(x => x.GroupName).ToList();
            ListGroupProduct.Insert(0, new GroupProduct() { GroupName = "Gốc" });
            ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName", groupProduct.IdRoot);
            return View(groupProduct);
        }

        // POST: Admin/GroupProducts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "GroupProductId,GroupName,SeoUrl,Order,IsDisable,ImgUrl,IdRoot")] GroupProduct groupProduct)
        {
            List<GroupProduct> ListGroupProduct = db.GroupProducts.Where(x => x.CountNode < 2).OrderBy(x => x.GroupName).ToList();
            ListGroupProduct.Insert(0, new GroupProduct() { GroupName = "Gốc" });

            if (ModelState.IsValid)
            {
                if (groupProduct.SeoUrl == null) { groupProduct.SeoUrl = DreamCMS.FuncHelp.DHelp.SEOurl(groupProduct.GroupName); }
                if (db.GroupProducts.Where(x => x.SeoUrl == groupProduct.SeoUrl && x.GroupProductId != groupProduct.GroupProductId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên nhóm đã được sử dụng!!!";
                    ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName", groupProduct.IdRoot);
                    return View(groupProduct);
                }

                if (groupProduct.IdRoot == 0) { groupProduct.IdRoot = null; groupProduct.CountNode = 0; }
                else
                {
                    GroupProduct gpRoot = db.GroupProducts.Where(x => x.GroupProductId == groupProduct.IdRoot).FirstOrDefault();
                    groupProduct.CountNode = gpRoot.CountNode + 1;
                }

                GroupProduct gp = db.GroupProducts.Where(x => x.GroupProductId == groupProduct.GroupProductId).FirstOrDefault();
                gp.GroupName = groupProduct.GroupName;
                gp.SeoUrl = groupProduct.SeoUrl;
                gp.CountNode = groupProduct.CountNode;
                gp.Order = groupProduct.Order;
                gp.IsDisable = groupProduct.IsDisable;
                gp.ImgUrl = groupProduct.ImgUrl;
                gp.IdRoot = groupProduct.IdRoot;
                //db.Entry(groupProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRoot = new SelectList(ListGroupProduct, "GroupProductId", "GroupName", groupProduct.IdRoot);
            return View(groupProduct);
        }

        // GET: Admin/GroupProducts/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            if (groupProduct == null)
            {
                return HttpNotFound();
            }
            return View(groupProduct);
        }

        // POST: Admin/GroupProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            GroupProduct groupProduct = db.GroupProducts.Find(id);
            Delete_Recur(db, groupProduct);
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

        private void Delete_Recur(DBFrontEnd db, GroupProduct gp)
        {
            List<GroupProduct> ListGroupProduct = gp.GroupProductChildrens.ToList();
            if(ListGroupProduct!=null)
            {
                foreach (var i in ListGroupProduct)
                    Delete_Recur(db, i);
            }

            List<Product> ListProduct = gp.Products.ToList();
            if (ListProduct != null)
            {
                foreach (var i in ListProduct)
                    db.Products.Remove(i);
                db.SaveChanges();
            }

            db.GroupProducts.Remove(gp);
            db.SaveChanges();

        }

    }
}
