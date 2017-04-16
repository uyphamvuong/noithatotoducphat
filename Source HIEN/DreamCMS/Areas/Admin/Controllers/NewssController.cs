using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class NewssController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Newss
        [Auth]
        public ActionResult Index()
        {
            return View(db.Newss.ToList());
        }

        // GET: Admin/Newss/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Newss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Create([Bind(Include = "NewsId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] News news)
        {
            if (ModelState.IsValid)
            {
                if (news.TitleId == null) { news.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(news.Title); }
                if (db.Newss.Where(x => x.TitleId == news.TitleId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(news);
                }

                db.Newss.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: Admin/Newss/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newss.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/Newss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Edit([Bind(Include = "NewsId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] News news)
        {
            if (ModelState.IsValid)
            {
                if (news.TitleId == null) { news.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(news.Title); }
                if (db.Newss.Where(x => x.TitleId == news.TitleId && x.NewsId != news.NewsId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(news);
                }

                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: Admin/Newss/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.Newss.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/Newss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.Newss.Find(id);
            db.Newss.Remove(news);
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
