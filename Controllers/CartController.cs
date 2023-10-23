using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Helpers;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Controllers
{
    public class CartController : Controller
    {
        private ShopDbContext _context;
        public CartController(ShopDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (ControllerContext.HttpContext.Session.Keys.Contains("cart"))
            {
                var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.cart = cart;
                ViewBag.totalPrice = cart.Sum(item => item.Product.Price * item.Quantity); 
            }
            
            return View();
        }

        public IActionResult Add(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                List<Item> cart = new();
                cart.Add(new Item { Product = _context.Products.Find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = IsExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new Item { Product = _context.Products.Find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            cart.RemoveAt(IsExist(id));
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");

            int buyerId = _context.Buyers.Where(b => b.Email == User.Identity.Name).Single().Id;

            foreach (var item in cart)
            {
                item.Product.Count -= item.Quantity;
                _context.Entry(item.Product).State = EntityState.Unchanged;

                Order order = new()
                {
                    BuyerFK = buyerId,
                    ProductFK = item.Product.Id,
                    Count = item.Quantity,
                    DateTime = DateTime.Now
                };

                _context.Orders.Add(order);
            }

            _context.SaveChanges();

            HttpContext.Session.Clear();

            return View();
        }

        private int IsExist(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
