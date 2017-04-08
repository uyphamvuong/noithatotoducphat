using DreamCMS.Authorize;
using DreamCMS.ModelsAdmin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class GroupsController : Controller
    {
        private DBAdmin db = new DBAdmin();

        // GET: Admin/Groups
        [Auth]
        public ActionResult Index()
        {
            return View("__Cms/Groups/Index", db.tbGroups.ToList());
        }

        // GET: Admin/Groups/Create
        [Auth]
        public ActionResult Create()
        {
            return View("__Cms/Groups/Create");
        }

        // POST: Admin/Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "tbGroupId,GroupName,Description,IsDisable")] tbGroup tbGroup)
        {
            if (ModelState.IsValid)
            {
                tbGroup tbGroupCheck = db.tbGroups.Where(p => p.GroupName == tbGroup.GroupName).FirstOrDefault();
                if (tbGroupCheck != null)
                {
                    ModelState.AddModelError("GroupNameValid", "Tên nhóm '" + tbGroup.GroupName + "' đã tồn tại!!!");
                    return View("__Cms/Groups/Create", tbGroup);
                }

                db.tbGroups.Add(tbGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("__Cms/Groups/Create", tbGroup);
        }

        // GET: Admin/Groups/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGroup tbGroup = db.tbGroups.Find(id);
            if (tbGroup == null)
            {
                return HttpNotFound();
            }
            return View("__Cms/Groups/Edit", tbGroup);
        }

        // POST: Admin/Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "tbGroupId,GroupName,Description,IsDisable")] tbGroup tbGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("__Cms/Groups/Edit", tbGroup);
        }

        // GET: Admin/Groups/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGroup tbGroup = db.tbGroups.Find(id);
            if (tbGroup == null)
            {
                return HttpNotFound();
            }
            return View("__Cms/Groups/Delete", tbGroup);
        }

        // POST: Admin/Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult DeleteConfirmed(int id)
        {
            tbGroup tbGroup = db.tbGroups.Find(id);
            db.tbGroups.Remove(tbGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Admin/Groups/ViewMember/5
        [Auth]
        public ActionResult ViewMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGroup tbGroup = db.tbGroups.Find(id);
            if (tbGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.tbGroup = tbGroup;
            List<tbUserInGroup> tbUserInGroup = new List<ModelsAdmin.tbUserInGroup>();
            db.tbUsers.ToList().ForEach(x => tbUserInGroup.Add(new tbUserInGroup(x, id.Value)));

            return View("__Cms/Groups/ViewMember", tbUserInGroup);
        }

        [HttpPost]
        [Auth]
        public ActionResult ChangeOneMember(int userid, int groupid, bool val)
        {
            tbGroupUser tbMemberGroup = db.tbGroupUsers.Where(x => x.tbUserId == userid && x.tbGroupId == groupid).FirstOrDefault();
            if(val)
            {                
                if (tbMemberGroup == null)
                {
                    tbGroupUser tbNew = new tbGroupUser();
                    tbNew.tbUserId = userid;
                    tbNew.tbGroupId = groupid;
                    tbNew.CreatedAt = DateTime.Now;
                    tbNew.CreatedBy = Auth.GetCookie("DUser");
                    db.tbGroupUsers.Add(tbNew);
                    db.SaveChanges();
                }
                return Json(new { result = true, msg = "Đã thêm thành viên vào nhóm thành công!" });
            }
            else
            {
                if (tbMemberGroup != null)
                {
                    db.tbGroupUsers.Remove(tbMemberGroup);
                    db.SaveChanges();
                }
                return Json(new { result = false, msg = "Đã xóa thành viên khỏi nhóm thành công!" });
            }
        }

        //IsGroupNameAvailble public class MyController : Controller
        //public ActionResult IsNameAvailble(string GroupName)
        //{
        //    if (Auth.IsUse(this, "Admin") != 1) { return Redirect(Auth.PathRedirect); }

        //    try
        //    {
        //        var tag = db.tbGroups.Single(m => m.GroupName == GroupName);
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Auth]
        public ActionResult PermissonGroup(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbGroup tbGroup = db.tbGroups.Find(id);
            if (tbGroup == null)
            {
                return HttpNotFound();
            }

            // Phân quyền tất cả
            //int coutPer = db.tbPermissonGroups.Where(x => x.tbGroupId == id).Count();
            //if (coutPer == 0)
            //{
            //    List<tbMenu> ListMenu = db.tbMenus.Where(x=>x.IsMenu).ToList();
            //    foreach (tbMenu item in ListMenu)
            //    {
            //        tbPermissonGroup tbNew = new tbPermissonGroup();
            //        tbNew.tbGroupId = tbGroup.tbGroupId;
            //        tbNew.tbMenuId = item.tbMenuId;
            //        db.tbPermissonGroups.Add(tbNew);
            //    }
            //    db.SaveChanges();
            //}

            ViewBag.tbGroup = tbGroup;
            List<tbMenuInGroup> tbMenuInGroup = new List<ModelsAdmin.tbMenuInGroup>();
            db.tbMenus.Where(x=>x.MenuName!="Root").ToList().ForEach(x => tbMenuInGroup.Add(new tbMenuInGroup(x, id.Value)));

            return View("__Cms/Groups/PermissonGroup", tbMenuInGroup);
        }

        [HttpPost]
        [Auth]
        public ActionResult ChangeOneMenu(int menuid, int groupid, bool val)
        {
            tbGroupMenu tbPermissonGroup = db.tbGroupMenus.Where(x => x.tbMenuId == menuid && x.tbGroupId == groupid).FirstOrDefault();
            if (val)
            {
                if (tbPermissonGroup == null)
                {
                    tbGroupMenu tbNew = new tbGroupMenu();
                    tbNew.tbMenuId = menuid;
                    tbNew.tbGroupId = groupid;
                    db.tbGroupMenus.Add(tbNew);
                    db.SaveChanges();
                }
                return Json(new { result = true, msg = "Đã thêm thành công!" });
            }
            else
            {
                if (tbPermissonGroup != null)
                {
                    db.tbGroupMenus.Remove(tbPermissonGroup);
                    db.SaveChanges();
                }
                return Json(new { result = false, msg = "Đã xóa thành công!" });
            }
        }

    }
}
