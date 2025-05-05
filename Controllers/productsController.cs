using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebClothesMVC.Models;

namespace WebClothesMVC.Controllers
{
    using System.IO;
    using WebClothesMVC.Models;
    public class productsController : Controller
    {
        private MyDatabaseCS _context = new MyDatabaseCS();
        public ActionResult Index()
        {
            // Validasi untuk admin atau cashier
            if (Session["role"] == null || (Session["role"].ToString() != "admin" && Session["role"].ToString() != "cashier"))
            {
                return RedirectToAction("Login", "Account");
            }

            var listofData = _context.products.ToList();
            return View(listofData);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(); // Harus cocok dengan Views/Home/Create.cshtml
        }
        [HttpPost]
        public ActionResult Create(product model, HttpPostedFileBase gambarFile)
        {
            if (ModelState.IsValid)
            {
                // Handle upload file gambar
                if (gambarFile != null && gambarFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(gambarFile.FileName);
                    string path = Server.MapPath("~/Content/Images/");

                    // Buat folder jika belum ada
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fullPath = Path.Combine(path, fileName);
                    gambarFile.SaveAs(fullPath);

                    model.gambar = fileName; // Simpan nama file ke database
                }

                // Isi otomatis updated_at
                model.updated_at = DateTime.Now;

                _context.products.Add(model);
                _context.SaveChanges();

                ViewBag.Message = "Data berhasil disimpan!";
                return RedirectToAction("Index"); // Atau arahkan ke halaman lain jika perlu      
            }

            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var data = _context.products.Where(x => x.id_product == id).FirstOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult Edit(product model, HttpPostedFileBase gambarFile)
        {
            var data = _context.products.FirstOrDefault(x => x.id_product == model.id_product);

            if (data != null)
            {
                data.nama_product = model.nama_product;
                data.quantity = model.quantity;
                data.price = model.price;

                if (gambarFile != null && gambarFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(gambarFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                    gambarFile.SaveAs(path);
                    data.gambar = fileName; // update nama file gambar
                }

                data.updated_at = DateTime.Now; // Update otomatis

                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details()
        {
            var product = _context.products.FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        public ActionResult Delete(int id)
        {
            var data = _context.products.Where(x => x.id_product == id).FirstOrDefault();
            _context.products.Remove(data);
            _context.SaveChanges();
            ViewBag.Messsage = "Record Delete Successfully";
            return RedirectToAction("index");
        }
    }
}
