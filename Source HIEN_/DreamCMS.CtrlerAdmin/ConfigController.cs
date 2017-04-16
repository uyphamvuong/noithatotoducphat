using DreamCMS.Authorize;
using DreamCMS.Config;
using DreamCMS.Encrypt;
using DreamCMS.ModelsAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DreamCMS.Areas.Admin.Controllers
{
    public class ConfigController : Controller
    {
        DBAdmin db = new DBAdmin();

        // GET: Admin/Config
        [Auth]
        public ActionResult Index()
        {
            return View("__Cms/Config/Index");
        }

        // GET: Admin/DecryptPass
        [Auth]
        public ActionResult DecryptPass()
        {
            return View("__Cms/Config/DecryptPass");
        }

        // POST: Admin/DecryptPass
        [HttpPost]
        [Auth]
        public ActionResult DecryptPass(string typepass, string content, string keypass)
        {
            if(typepass=="d")
            {
                ViewBag.StrEncrypt = content;
                if (string.IsNullOrEmpty(keypass)){ViewBag.StrDecrypt = DHash.Decrypt(content);}else{ ViewBag.StrDePass = keypass; ViewBag.StrDecrypt = DHash.Decrypt(content, keypass);}
            }
            else if (typepass == "e")
            {
                ViewBag.StrDecrypt = content;
                if (string.IsNullOrEmpty(keypass)) { ViewBag.StrEncrypt = DHash.Encrypt(content); } else { ViewBag.StrEnPass = keypass; ViewBag.StrEncrypt = DHash.Encrypt(content, keypass); }
            }

            return View("__Cms/Config/DecryptPass");
        }
        
        public Dictionary<Type, IEnumerable<MethodInfo>> GetMvcActionMethods()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies(); // currently loaded assemblies
            var controllerTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t != null
                    && t.IsPublic // public controllers only
                    && t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase) // enfore naming convention
                    && !t.IsAbstract // no abstract controllers
                    && typeof(IController).IsAssignableFrom(t)).OrderBy(p=>p.ToString()); // should implement IController (happens automatically when you extend Controller)
            var controllerMethods = controllerTypes.ToDictionary(
                controllerType => controllerType,
                controllerType => controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType)));
            return controllerMethods;
        }

        [Auth]
        public ActionResult MenuInit()
        {
            Dictionary<Type, IEnumerable<MethodInfo>> Ctrl = GetMvcActionMethods();
            ViewBag.Ctrl = Ctrl;

            return View("__Cms/Config/MenuInit");
        }

        
        [HttpPost]
        [Auth]
        public ActionResult MenuCreate()
        {
            List<tbGroupMenu> arrper = db.tbGroupMenus.ToList();
            foreach (tbGroupMenu item in arrper)
                db.tbGroupMenus.Remove(item);
            db.SaveChanges();
            
            List<tbMenu> arrmenu = db.tbMenus.Where(p => p.IdRoot == null).ToList();
            foreach (tbMenu item in arrmenu)
                DeleleRecursion_tbMenu(db, item);
            db.SaveChanges();

            tbMenu tbMenuRoot = new tbMenu();
            tbMenuRoot.MenuName = "Root"; // Cpanel
            tbMenuRoot.IsController = false;
            tbMenuRoot.IsMenu = false;
            db.tbMenus.Add(tbMenuRoot);
            db.SaveChanges();

            Dictionary<Type, IEnumerable<MethodInfo>> Ctrl = GetMvcActionMethods();
            foreach (KeyValuePair<Type, IEnumerable<MethodInfo>> i in Ctrl)
            {
                string[] arr = i.Key.ToString().Split('.');

                string controller = arr[arr.Length - 1].Replace("Controller", "");
                string area = arr[1] == "Areas" ? arr[2] : null;

                if (controller == "Error") { continue; }

                tbMenu tbMenuNew = new tbMenu();
                tbMenuNew.MenuName = controller; // Cpanel
                tbMenuNew.Controller = controller;
                tbMenuNew.Area = area;
                tbMenuNew.ClassIcon = "fa fa-folder-open";
                tbMenuNew.IsController = true;
                tbMenuNew.IsMenu = area==null?false:true;
                tbMenuNew.IdRoot = tbMenuRoot.tbMenuId;
                db.tbMenus.Add(tbMenuNew);
                db.SaveChanges();

                foreach (MethodInfo j in i.Value.ToList())
                {
                    tbMenu checkmenu = db.tbMenus.Where(p => p.Controller == controller && p.Action == j.Name && p.Area == area).FirstOrDefault();
                    if (checkmenu != null) { continue; }
                    tbMenu tbNew = new tbMenu();
                    tbNew.MenuName = j.Name; // Cpanel-Index
                    tbNew.Controller = controller;
                    tbNew.Action = j.Name;
                    tbNew.Area = area;
                    tbNew.IsController = false;
                    tbNew.IsMenu = area == null ? false : true;
                    if (j.Name == "Details" || j.Name == "Edit" || j.Name == "Delete" || j.Name == "DeleteConfirmed") { tbNew.IsMenu = false; }                    
                    tbNew.IdRoot = tbMenuNew.tbMenuId;
                    db.tbMenus.Add(tbNew);
                    db.SaveChanges();
                }
            }

            if (!DConfig.IsCheck) { DConfig.SetValue("DateCreateMenu", DateTime.Now.ToString()); }
            else { DConfig.SetValue("DateModifyMenu", DateTime.Now.ToString()); }

            return RedirectToAction("Index","Menus");
        }

        #region # Fucntion Private #

        private void DeleleRecursion_tbMenu(DBAdmin db, tbMenu rd)
        {
            List<tbMenu> arrmenu = db.tbMenus.Where(x => x.IdRoot == rd.tbMenuId).ToList();

            if (arrmenu.Count() > 0)
                foreach (var c in arrmenu)
                    this.DeleleRecursion_tbMenu(db,c);

            db.tbMenus.Remove(rd);
            db.SaveChanges();
        }

        #endregion

    }
}