using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Views
{
    public class SliderImgsController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/SliderImgs
        [Auth]
        public ActionResult Index()
        {
            return View(db.SliderImgs.ToList());
        }

        // GET: Admin/SliderImgs/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/SliderImgs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "SliderImgId,TitleImg,Order,ImgUrl,Link")] SliderImg sliderImg)
        {
            if (ModelState.IsValid)
            {
                if (db.SliderImgs.Where(x => x.TitleImg== sliderImg.TitleImg).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tiêu đề hình ảnh đã được sử dụng!!!";
                    return View(sliderImg);
                }

                db.SliderImgs.Add(sliderImg);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sliderImg);
        }

        // GET: Admin/SliderImgs/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderImg sliderImg = db.SliderImgs.Find(id);
            if (sliderImg == null)
            {
                return HttpNotFound();
            }
            return View(sliderImg);
        }

        // POST: Admin/SliderImgs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "SliderImgId,TitleImg,Order,ImgUrl,Link")] SliderImg sliderImg)
        {
            if (ModelState.IsValid)
            {
                if (db.SliderImgs.Where(x => x.TitleImg == sliderImg.TitleImg && x.SliderImgId!=sliderImg.SliderImgId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tiêu đề hình ảnh đã được sử dụng!!!";
                    return View(sliderImg);
                }

                db.Entry(sliderImg).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sliderImg);
        }

        // GET: Admin/SliderImgs/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SliderImg sliderImg = db.SliderImgs.Find(id);
            if (sliderImg == null)
            {
                return HttpNotFound();
            }
            return View(sliderImg);
        }

        // POST: Admin/SliderImgs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            SliderImg sliderImg = db.SliderImgs.Find(id);
            db.SliderImgs.Remove(sliderImg);
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
