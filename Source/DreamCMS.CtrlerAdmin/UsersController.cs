using DreamCMS.Authorize;
using DreamCMS.ModelsAdmin;
using DreamCMS.Upload;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private DBAdmin db = new DBAdmin();

        // GET: Admin/Users
        [Auth]
        public ActionResult Index()
        {
            return View("__Cms/Users/Index", db.tbUsers.ToList());
        }

        // GET: Admin/Users/Create
        [Auth]
        public ActionResult Create()
        {
            return View("__Cms/Users/Create");
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "tbUserId,Username,Fullname,Password,IsDisable")] tbUser tbUser, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                tbUser tbUserCheck = db.tbUsers.Where(p => p.Username == tbUser.Username).FirstOrDefault();
                if (tbUserCheck != null) {
                    ModelState.AddModelError("UserNameValid","Tài khoản '" + tbUser.Username + "' đã tồn tại!!!");
                    return View(tbUser);
                }

                tbUser.Password = DreamCMS.Encrypt.DHash.Encrypt(tbUser.Password);
                db.tbUsers.Add(tbUser);
                db.SaveChanges();

                // AVATAR
                if (file != null)
                {
                    if (HttpPostedFileBaseExtensions.IsImage(file))
                    {
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //string pic = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUser.Username+ext);
                        // file is uploaded
                        file.SaveAs(path);

                        tbUser.AvatarUrl = tbUser.Username + ext;
                        db.Entry(tbUser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index");
            }

            return View("__Cms/Users/Create", tbUser);
        }

        // GET: Admin/Users/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUser tbUser = db.tbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            tbUser.Password = DreamCMS.Encrypt.DHash.Decrypt(tbUser.Password);
            return View("__Cms/Users/Edit", tbUser);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "tbUserId,Username,Fullname,Password,IsDisable")] tbUser tbUser, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                tbUser.Password = DreamCMS.Encrypt.DHash.Encrypt(tbUser.Password);                

                // AVATAR
                if (file != null)
                {
                    if (HttpPostedFileBaseExtensions.IsImage(file))
                    {
                        string ext = Path.GetExtension(file.FileName).ToLower();
                        //string pic = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUser.Username + ext);
                        //delele old file
                        if(!string.IsNullOrEmpty(tbUser.AvatarUrl))
                        {
                            string fullPath = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUser.AvatarUrl);
                            if (System.IO.File.Exists(fullPath))
                            {
                                System.IO.File.Delete(fullPath);
                            }
                        }
                        // file is uploaded
                        file.SaveAs(path);

                        tbUser.AvatarUrl = tbUser.Username + ext;
                    }
                }

                db.Entry(tbUser).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View("__Cms/Users/Edit", tbUser);
        }

        // GET: Admin/Users/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbUser tbUser = db.tbUsers.Find(id);
            if (tbUser == null)
            {
                return HttpNotFound();
            }
            return View("__Cms/Users/Delete", tbUser);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult DeleteConfirmed(int id)
        {
            tbUser tbUser = db.tbUsers.Find(id);

            //delele old file
            if (!string.IsNullOrEmpty(tbUser.AvatarUrl))
            {
                string fullPath = Path.Combine(Server.MapPath("~/Areas/Admin/upload/avatar"), tbUser.AvatarUrl);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }

            db.tbUsers.Remove(tbUser);
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

        //public ActionResult IsNameAvailble(string Username)
        //{
        //    try
        //    {
        //        var tag = db.tbUsers.Single(m => m.Username == Username);
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //}

    }
}
