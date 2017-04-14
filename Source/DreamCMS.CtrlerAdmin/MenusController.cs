using DreamCMS.Authorize;
using DreamCMS.ModelsAdmin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class MenusController : Controller
    {
        private DBAdmin db = new DBAdmin();

        // GET: Admin/Menus
        [Auth]
        public ActionResult Index()
        {
            var tbMenus = db.tbMenus.Include(t => t.tbMenuParrent);
            return View("__Cms/Menus/Index", tbMenus.ToList());
        }

        // GET: Admin/Menus/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.IdRoot = new SelectList(db.tbMenus, "tbMenuId", "MenuName");
            return View("__Cms/Menus/Create");
        }

        // POST: Admin/Menus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Create([Bind(Include = "tbMenuId,MenuName,Controller,Action,Area,QueryString,ClassIcon,Order,IsController,IsMenu,IdRoot,IsDisable")] tbMenu tbMenu)
        {
            if (ModelState.IsValid)
            {
                db.tbMenus.Add(tbMenu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdRoot = new SelectList(db.tbMenus, "tbMenuId", "MenuName", tbMenu.IdRoot);
            return View("__Cms/Menus/Create", tbMenu);
        }

        // GET: Admin/Menus/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbMenu tbMenu = db.tbMenus.Find(id);
            if (tbMenu == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRoot = new SelectList(db.tbMenus, "tbMenuId", "MenuName", tbMenu.IdRoot);
            return View("__Cms/Menus/Edit", tbMenu);
        }

        // POST: Admin/Menus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult Edit([Bind(Include = "tbMenuId,MenuName,Controller,Action,Area,QueryString,ClassIcon,Order,IsController,IsMenu,IdRoot,IsDisable")] tbMenu tbMenu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbMenu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRoot = new SelectList(db.tbMenus, "tbMenuId", "MenuName", tbMenu.IdRoot);
            return View("__Cms/Menus/Edit", tbMenu);
        }

        // GET: Admin/Menus/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbMenu tbMenu = db.tbMenus.Find(id);
            if (tbMenu == null)
            {
                return HttpNotFound();
            }
            return View("__Cms/Menus/Delete", tbMenu);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth]
        public ActionResult DeleteConfirmed(int id)
        {
            tbMenu tbMenu = db.tbMenus.Find(id);
            db.tbMenus.Remove(tbMenu);
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

        [HttpPost]
        [Auth(IsNotLayout=true)]
        public JsonResult UpdateMenuOneBool(int menuid, bool val, string strtype)
        {
            tbMenu tbMenu = db.tbMenus.Where(x => x.tbMenuId == menuid).FirstOrDefault();
            if (tbMenu == null)
            {
                return Json(new { result = false, msg = "Menu không còn tồn tại >.<" });
            }
            else
            {
                switch(strtype)
                {
                    case "ctrl":
                        tbMenu.IsController = val;
                        break;
                    case "mn":
                        tbMenu.IsMenu = val;
                        break;
                    case "dis":
                        tbMenu.IsDisable = val;
                        break;
                    default:
                        return Json(new { result = false, msg = "Lỗi không xác định. Cập nhật dữ liệu thất bại!" });
                }
                db.SaveChanges();
                return Json(new { result = val, msg = "Đã update \"" + tbMenu.MenuName + "[" + strtype + "]\"" });
            }
        }

        [HttpPost]
        [Auth(IsNotLayout = true)]
        public ActionResult UpdateMenuOneName(int pk, string value, string name)
        {
            tbMenu tbMenu = db.tbMenus.Where(x => x.tbMenuId == pk).FirstOrDefault();
            if (tbMenu == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Menu không tồn tại"));
            }
            else
            {
                try
                {
                    tbMenu.MenuName = value;
                    db.SaveChanges();
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (DbEntityValidationException ex)
                {
                    var error = ex.EntityValidationErrors.First().ValidationErrors.First();
                    //this.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, error.ErrorMessage);
                }
                
            }
        }

        [Auth]
        public ActionResult SortMenu()
        {
            tbMenu root = db.tbMenus.Where(p => p.MenuName == "Root").FirstOrDefault();

            List<tbMenu> ListMenu = new List<tbMenu>();
            if (root != null)
            {
                ListMenu = db.tbMenus.Where(p => p.IdRoot == root.tbMenuId && p.IsDisable == false && p.IsMenu == true).OrderBy(x => x.Order).ToList();
            }

            return View("__Cms/Menus/SortMenu", ListMenu);
        }

        [HttpPost]
        [Auth(IsNotLayout = true)]
        public JsonResult SortMenu(string dataAjax)
        {
            try
            {
                List<dataAjaxtSort> data = new JavaScriptSerializer().Deserialize<List<dataAjaxtSort>>(dataAjax);

                tbMenu root = db.tbMenus.Where(p => p.MenuName == "Root").FirstOrDefault();

                if(root!=null)
                    foreach (dataAjaxtSort c in data)
                        UpdateRecursion_tbMenu(c, root.tbMenuId);
                return Json(new { result = true, msg = "Cập nhật dữ liệu Thành Công :) " });
            }
            catch
            {
                return Json(new { result = false, msg = "Cập nhật dữ liệu thất bại!" });
            }
            
        }

        private int sortid = 0;
        private void UpdateRecursion_tbMenu(dataAjaxtSort rd, int idroot)
        {
            tbMenu tbMenu = db.tbMenus.Where(x => x.tbMenuId == rd.id).FirstOrDefault();
            if(tbMenu!=null)
            {
                tbMenu.IdRoot = idroot;
                tbMenu.Order = sortid;
                db.SaveChanges();
                sortid++;

                if (rd.children != null)
                    foreach (dataAjaxtSort c in rd.children)
                        UpdateRecursion_tbMenu(c, tbMenu.tbMenuId);
            }            
        }

        class dataAjaxtSort
        {
            public int id{get;set;}
            public List<dataAjaxtSort> children{get;set;}
        }

    }
}
