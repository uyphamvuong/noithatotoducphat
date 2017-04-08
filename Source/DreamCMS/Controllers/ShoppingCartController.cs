using DreamCMS.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamCMS.Controllers
{
    public class ShoppingCartController : Controller
    {

        private DBFrontEnd db = new DBFrontEnd();
        public ActionResult Index()
        {
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];
            
            return View(model);
        }


        public JsonResult AddToCart(int id)
        {
            var response = new { Code = 1, Msg = "Fail", Dem = 0 };
            var objProduct = db.Products.Where(p => p.ProductId == id).FirstOrDefault();
            if (objProduct != null)
            {
                ShoppingCart objCart = (ShoppingCart)Session["Cart"];
                if (objCart == null)
                {
                    objCart = new ShoppingCart();
                }

                ShoppingCartItem item = new ShoppingCartItem()
                {
                    ProductID = objProduct.ProductId,
                    ProductName = objProduct.ProductName,
                    ProductImage = objProduct.ImgUrl,
                };

                objCart.AddToCart(item);
                Session["Cart"] = objCart;
                int tong_soluong = objCart.ListItem != null ? objCart.ListItem.Count : 0;
                response = new { Code = 0, Msg = "Success", Dem = tong_soluong };
            }
            return Json(response);
        }


        [HttpPost]
        public JsonResult RemoveFromCart(int id)
        {
            var response = new { Code = 1, Msg = "Fail" };

            ShoppingCart objCart = (ShoppingCart)Session["Cart"];
            if (objCart != null)
            {
                objCart.RemoveFromCart(id);
                Session["Cart"] = objCart;
                response = new { Code = 0, Msg = "Success" };
            }
            return Json(response);
        }


        public Product GetProductByID(int id)
        {
            return new Product()
            {
                ProductId = id,
                ProductName = String.Format("{0}", id),
                ImgUrl = String.Format("{0}", id),
            };
        }


        public string ThanhToan(string Name, string Email, string SDT, string Adress, string Des)
        {
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];

            Order or = new Order();
            or.Name = Name;
            or.Email = Email;
            or.Date = DateTime.Now;
            or.SDT = SDT;
            or.Adress = Adress;
            or.Des = Des;
            db.Orders.Add(or);
            db.SaveChanges();

            foreach (var item in model.Cart.ListItem)
            {
                OrderDetail det = new OrderDetail();
                det.OrderId = or.OrderId;
                det.ProductId = Convert.ToInt32(item.ProductID);
                det.ProductName = HttpUtility.UrlEncode(item.ProductName);
                det.ImgUrl = item.ProductImage;
                db.OrderDetails.Add(det);
                db.SaveChanges();
            }
            Session["Cart"] = null;   

            return "Successful";
        }

        public ActionResult menucart()
        {
            ShoppingCartModels model = new ShoppingCartModels();
            model.Cart = (ShoppingCart)Session["Cart"];
            ViewBag.dem = 0;
            int dem = 0;
            if (model.Cart != null)
            {
                foreach (var item in model.Cart.ListItem)
                {
                    dem++;
                }
            }
            if (dem > 0)
            {
                ViewBag.dem = dem;
            }
            return PartialView();
        }

    }
}