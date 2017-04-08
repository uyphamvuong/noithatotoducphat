using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Posts
        [Auth]
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        // GET: Admin/Posts/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Create([Bind(Include = "PostId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                if (post.TitleId == null) { post.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(post.Title); }
                if (db.Posts.Where(x => x.TitleId == post.TitleId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(post);
                }

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(post);
        }

        // GET: Admin/Posts/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Edit([Bind(Include = "PostId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] Post post)
        {
            if (ModelState.IsValid)
            {
                if (post.TitleId == null) { post.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(post.Title); }
                if (db.Posts.Where(x => x.TitleId == post.TitleId && x.PostId != post.PostId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên bài viết đã được sử dụng!!!";
                    return View(post);
                }

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        // GET: Admin/Posts/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            db.Posts.Remove(post);
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
