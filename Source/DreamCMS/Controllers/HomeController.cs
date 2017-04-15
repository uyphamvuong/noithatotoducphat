using DreamCMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace DreamCMS.Controllers
{
    public class HomeController : Controller
    {
        private DBFrontEnd db = new DBFrontEnd();

        public ActionResult Index()
        {
            // Giới thiệu
            Page gt = db.Pages.Where(x => x.TitleId == "gioi-thieu").FirstOrDefault();
            ViewBag.GioiThieu = gt;

            // 8 sản phẩm
            List<Product> ListProduct = db.Products.Include(p => p.GroupProduct).Where(x => x.IsDisable == false).OrderByDescending(x => x.Order).Take(8).ToList();
            if (ListProduct == null) { ListProduct = new List<Product>(); }
            ViewBag.ListProduct = ListProduct;

            // 9 hoạt động
            List<Post> ListPost = db.Posts.Where(x => x.IsDisable == false).OrderByDescending(x => x.PostId).Take(9).ToList();
            if (ListPost == null) { ListPost = new List<Post>(); }
            ViewBag.ListPost = ListPost;

            // Đối tác
            List<Partner> ListPartner = db.Partners.ToList();
            if (ListPartner == null) { ListPartner = new List<Partner>();}
            ViewBag.ListPartner = ListPartner;


            return View();
        }

        public ActionResult ViewPages(string titleid)
        {
            Page page = db.Pages.Where(x => x.TitleId == titleid && x.IsDisable == false).FirstOrDefault();
            if (page == null)
            {
                return RedirectToAction("HttpError404", "Error");
            }
            return View(page);
        }

        public ActionResult Contact()
        {            
            Random newrd = new Random();
            Session.Add("CodeAuthContact", newrd.Next(10000, 99999));
            ViewBag.IsSuccess = false;
            return View();
        }

        [HttpPost]
        public ActionResult Contact([Bind(Include = "ContactId,FullName,Address,Email,Phone,Content")] Contact contact, string codeauth)
        {
            if (ModelState.IsValid)
            {

                string CodeAuthContact = Session["CodeAuthContact"].ToString();

                if(string.IsNullOrEmpty(CodeAuthContact) || string.IsNullOrEmpty(codeauth)){
                    ViewBag.ErrorCodeAuth = "Mã xác nhận không trùng khớp";
                    ViewBag.IsSuccess = false;
                    return View(contact);
                }
                else if(CodeAuthContact!=codeauth)
                {
                    ViewBag.ErrorCodeAuth = "Mã xác nhận không trùng khớp";
                    ViewBag.IsSuccess = false;
                    return View(contact);
                }

                db.Contacts.Add(contact);
                db.SaveChanges();

                ViewBag.IsSuccess = true;

                string emailrc = Setting.Get("EmailReceiveContact");
                if (emailrc.Length > 0)
                {
                    //sendmail
                    string content = FuncHelp.DHelp.Email.TemplateMail("Thư gửi từ BaoBiToanQuoc",
                        "Thư gửi từ BaoBiToanQuoc",
                        contact.ContactId.ToString(),
                        contact.FullName,
                        contact.Address,
                        contact.Email,
                        contact.Phone,
                        contact.Content);
                    FuncHelp.DHelp.Email.SendMail(emailrc, "[BaoBiToanQuoc] #"+contact.ContactId.ToString()+" Thư từ "+contact.FullName, content);
                }                
                
                return View(contact);
            }
            ViewBag.IsSuccess = false;
            return View(contact);
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