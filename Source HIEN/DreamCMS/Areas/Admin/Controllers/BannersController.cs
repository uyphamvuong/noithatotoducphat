using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Views
{
    public class BannersController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Banners
        [Auth]
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }

        // GET: Admin/Banners/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "BannerId,TitleImg,Order,ImgUrl,Link")] Banner Banner)
        {
            if (ModelState.IsValid)
            {
                db.Banners.Add(Banner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Banner);
        }

        // GET: Admin/Banners/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner Banner = db.Banners.Find(id);
            if (Banner == null)
            {
                return HttpNotFound();
            }
            return View(Banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "BannerId,TitleImg,Order,ImgUrl,Link")] Banner Banner)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Banner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Banner);
        }

        // GET: Admin/Banners/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner Banner = db.Banners.Find(id);
            if (Banner == null)
            {
                return HttpNotFound();
            }
            return View(Banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner Banner = db.Banners.Find(id);
            db.Banners.Remove(Banner);
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
