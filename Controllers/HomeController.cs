using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebClothesMVC.Controllers
{
    using System.IO;
    using WebClothesMVC.Models;
    public class HomeController : Controller
    {
        private MyDatabaseCS _context = new MyDatabaseCS();
        public ActionResult Index()
        {
            // Validasi untuk customer
            if (Session["role"] == null || Session["role"].ToString() != "customer")
            {
                return RedirectToAction("Login", "Account");
            }

            var ListofData = _context.products.ToList();
            return View(ListofData);
        }
    }
}