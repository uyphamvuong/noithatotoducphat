using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class PartnersController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Partners
        [Auth]
        public ActionResult Index()
        {
            return View(db.Partners.ToList());
        }

        // GET: Admin/Partners/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "PartnerId,CompanytName,ImgUrl,Link")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                if (db.Partners.Where(x => x.CompanytName == partner.CompanytName).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên công ty đã được sử dụng!!!";
                    return View(partner);
                }
                db.Partners.Add(partner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(partner);
        }

        // GET: Admin/Partners/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = db.Partners.Find(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // POST: Admin/Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "PartnerId,CompanytName,ImgUrl,Link")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                if (db.Partners.Where(x => x.CompanytName == partner.CompanytName && x.PartnerId != partner.PartnerId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên công ty đã được sử dụng!!!";
                    return View(partner);
                }

                db.Entry(partner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(partner);
        }

        // GET: Admin/Partners/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = db.Partners.Find(id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // POST: Admin/Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Partner partner = db.Partners.Find(id);
            db.Partners.Remove(partner);
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
