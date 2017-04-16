using System.Collections.Generic;
using System.Linq;

namespace DreamCMS.Models
{
    
        public class ShoppingCartItem
        {
            public long ProductID { get; set; }
            public string ProductName { get; set; }
            public string ProductImage { get; set; }
        }

        public class ShoppingCart
        {
            public ShoppingCart()
            {
                ListItem = new List<ShoppingCartItem>();
            }

            public List<ShoppingCartItem> ListItem { get; set; }

            public bool AddToCart(ShoppingCartItem item)
            {
                bool alreadyExists = ListItem.Any(x => x.ProductID == item.ProductID);
                if (alreadyExists)
                {
                    ShoppingCartItem existsItem = ListItem.Where(x => x.ProductID == item.ProductID).SingleOrDefault();
                }
                else
                {
                    ListItem.Add(item);
                }
                return true;
            }

            public bool RemoveFromCart(long lngProductSellID)
            {
                ShoppingCartItem existsItem = ListItem.Where(x => x.ProductID == lngProductSellID).SingleOrDefault();
                if (existsItem != null)
                {
                    ListItem.Remove(existsItem);
                }
                return true;
            }



            public bool EmptyCart()
            {
                ListItem.Clear();
                return true;
            }
        }
    
}