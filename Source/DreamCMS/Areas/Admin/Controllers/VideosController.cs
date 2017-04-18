using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class VideosController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Videoss
        [Auth]
        public ActionResult Index()
        {
            return View(db.Videos.ToList());
        }

        // GET: Admin/Videoss/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Videoss/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Create([Bind(Include = "VideoId,Title,TitleId,Intro,Keyword,Link,Context,IsDisable,ImgUrl,IdText")] Video Videos)
        {
            if (ModelState.IsValid)
            {
                if (Videos.TitleId == null) { Videos.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(Videos.Title); }
                if (db.Videos.Where(x => x.TitleId == Videos.TitleId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(Videos);
                }

                if (!string.IsNullOrEmpty(Videos.Link))
                    Videos.IdText = Videos.Link.Substring(Videos.Link.IndexOf("?v=") + 3, Videos.Link.Length - Videos.Link.IndexOf("?v=") - 3);
                if (string.IsNullOrEmpty(Videos.ImgUrl))
                    Videos.ImgUrl = "https://img.youtube.com/vi/" + Videos.IdText + "/mqdefault.jpg";

                db.Videos.Add(Videos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Videos);
        }

        // GET: Admin/Videoss/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video Videos = db.Videos.Find(id);
            if (Videos == null)
            {
                return HttpNotFound();
            }
            return View(Videos);
        }

        // POST: Admin/Videoss/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Edit([Bind(Include = "VideoId,Title,TitleId,Intro,Keyword,Context,Link,IsDisable,ImgUrl,IdText")] Video Videos)
        {
            if (ModelState.IsValid)
            {
                if (Videos.TitleId == null) { Videos.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(Videos.Title); }
                if (db.Videos.Where(x => x.TitleId == Videos.TitleId && x.VideoId != Videos.VideoId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(Videos);
                }
                if (!string.IsNullOrEmpty(Videos.Link))
                    Videos.IdText = Videos.Link.Substring(Videos.Link.IndexOf("?v=") + 3, Videos.Link.Length - Videos.Link.IndexOf("?v=") - 3);
                if (string.IsNullOrEmpty(Videos.ImgUrl))
                    Videos.ImgUrl = "https://img.youtube.com/vi/" + Videos.IdText + "/mqdefault.jpg";                 

                db.Entry(Videos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Videos);
        }

        // GET: Admin/Videoss/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video Videos = db.Videos.Find(id);
            if (Videos == null)
            {
                return HttpNotFound();
            }
            return View(Videos);
        }

        // POST: Admin/Videoss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout = true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Video Videos = db.Videos.Find(id);
            db.Videos.Remove(Videos);
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
