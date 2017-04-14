using DreamCMS.Authorize;
using DreamCMS.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        // GET: Admin/Pages
        [Auth]
        public ActionResult Index()
        {
            return View(db.Pages.ToList());
        }

        // GET: Admin/Pages/Create
        [Auth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Pages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Create([Bind(Include = "PageId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] Page page)
        {
            if (ModelState.IsValid)
            {
                if (page.TitleId == null) { page.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(page.Title); }
                if (db.Pages.Where(x => x.TitleId == page.TitleId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên trang đã được sử dụng!!!";
                    return View(page);
                }

                db.Pages.Add(page);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(page);
        }

        // GET: Admin/Pages/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Admin/Pages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        [Auth]
        public ActionResult Edit([Bind(Include = "PageId,Title,TitleId,Intro,Keyword,Context,IsDisable,ImgUrl")] Page page)
        {
            if (ModelState.IsValid)
            {
                if (page.TitleId == null) { page.TitleId = DreamCMS.FuncHelp.DHelp.SEOurl(page.Title); }
                if (db.Pages.Where(x => x.TitleId == page.TitleId && x.PageId != page.PageId).FirstOrDefault() != null)
                {
                    ViewBag.IsValidName = "Tên trang đã được sử dụng!!!";
                    return View(page);
                }

                db.Entry(page).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.IsSuccess = true;
                ViewBag.IsValidName = "Cập nhật dữ liệu thành công!!!";
                return View(page);
            }
            return View(page);
        }

        // GET: Admin/Pages/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Page page = db.Pages.Find(id);
            if (page == null)
            {
                return HttpNotFound();
            }
            return View(page);
        }

        // POST: Admin/Pages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(IsNotLayout=true)]
        public ActionResult DeleteConfirmed(int id)
        {
            Page page = db.Pages.Find(id);
            db.Pages.Remove(page);
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
