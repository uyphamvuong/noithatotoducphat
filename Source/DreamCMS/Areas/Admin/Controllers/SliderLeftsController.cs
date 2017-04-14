using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Views
{
    public class SliderLeftsController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/SliderLefts
        [Auth]
        public ActionResult Index()
        {
            return View(db.SliderLefts.ToList());
        }

        // GET: Admin/SliderLefts/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SliderLefts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "SliderLeftId,TitleImg,Order,ImgUrl,Link")] SliderLeft sliderLeft)
        {
            if (ModelState.IsValid)
            {
                db.SliderLefts.Add(sliderLeft);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sliderLeft);
        }

        // GET: Admin/SliderLefts/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderLeft sliderLeft = db.SliderLefts.Find(id);
            if (sliderLeft == null)
            {
                return HttpNotFound();
            }
            return View(sliderLeft);
        }

        // POST: Admin/SliderLefts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "SliderLeftId,TitleImg,Order,ImgUrl,Link")] SliderLeft sliderLeft)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sliderLeft).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sliderLeft);
        }

        // GET: Admin/SliderLefts/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderLeft sliderLeft = db.SliderLefts.Find(id);
            if (sliderLeft == null)
            {
                return HttpNotFound();
            }
            return View(sliderLeft);
        }

        // POST: Admin/SliderLefts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            SliderLeft sliderLeft = db.SliderLefts.Find(id);
            db.SliderLefts.Remove(sliderLeft);
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
