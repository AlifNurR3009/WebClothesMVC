using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebClothesMVC.Models;

namespace WebClothesMVC.Controllers
{
    public class CartController : Controller
    {
        private MyDatabaseCS db = new MyDatabaseCS();

        // GET: Cart
        public ActionResult Index()
        {
            if (Session["username"] == null || Session["role"].ToString() != "customer")
                return RedirectToAction("Login", "Account");

            string username = Session["username"].ToString();

            var cartItems = db.carts
                .Where(c => c.username_customer == username)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        public ActionResult AddToCart(int id_product, int quantity)
        {
            if (Session["username"] == null || Session["role"].ToString() != "customer")
                return RedirectToAction("Login", "Account");

            string username = Session["username"].ToString();

            var existingItem = db.carts.FirstOrDefault(c => c.username_customer == username && c.id_product == id_product);

            if (existingItem != null)
            {
                existingItem.quantity += quantity;
                existingItem.created_at = DateTime.Now;
            }
            else
            {
                var newCart = new cart
                {
                    username_customer = username,
                    id_product = id_product,
                    quantity = quantity,
                    created_at = DateTime.Now
                };
                db.carts.Add(newCart);
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UpdateAll(List<cart> carts)
        {

            if (carts == null || carts.Count == 0)
            {
                // Mengarahkan ke halaman error atau menampilkan notifikasi jika carts null atau kosong
                TempData["NoChanges"] = "Tidak ada perubahan atau data keranjang kosong.";
                return RedirectToAction("Index");
            }

            bool isUpdated = false;

            foreach (var item in carts)
            {
                var existingItem = db.carts.Find(item.id_cart);
                if (existingItem != null && item.quantity > 0)
                {
                    if (existingItem.quantity != item.quantity)
                    {
                        existingItem.quantity = item.quantity;
                        existingItem.created_at = DateTime.Now;
                        isUpdated = true;
                    }
                }
            }

            db.SaveChanges();

            if (!isUpdated)
            {
                TempData["NoChanges"] = "Tidak ada perubahan pada kuantitas.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult UpdateAll(List<cart> carts)
        //{
        //    if (carts == null)
        //    {
        //        // Tambahkan log untuk debugging
        //        Console.WriteLine("carts is null");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"carts count: {carts.Count}");
        //    }

        //    // Sisa kode seperti sebelumnya...
        //}


        public ActionResult Delete(int id)
        {
            var item = db.carts.Find(id);
            if (item != null)
            {
                db.carts.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
