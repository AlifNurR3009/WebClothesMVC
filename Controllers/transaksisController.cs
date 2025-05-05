using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebClothesMVC.Models;

namespace WebClothesMVC.Controllers
{
    public class transaksisController : Controller
    {
        private MyDatabaseCS db = new MyDatabaseCS();

        // GET: transaksis
        public ActionResult Index()
        {
            var transaksis = db.transaksis.Include(t => t.cart).Include(t => t.employe);
            return View(transaksis.ToList());
        }

        // GET: transaksis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaksi transaksi = db.transaksis.Find(id);
            if (transaksi == null)
            {
                return HttpNotFound();
            }
            return View(transaksi);
        }

        // GET: transaksis/Create
        public ActionResult Create()
        {
            ViewBag.id_cart = new SelectList(db.carts, "id_cart", "username_customer");
            ViewBag.id_employe = new SelectList(db.employes, "id_employe", "email_employe");
            return View();
        }

        // POST: transaksis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_transaksi,QOH,subtotal,id_employe,id_cart,created_at")] transaksi transaksi)
        {
            if (ModelState.IsValid)
            {
                db.transaksis.Add(transaksi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_cart = new SelectList(db.carts, "id_cart", "username_customer", transaksi.id_cart);
            ViewBag.id_employe = new SelectList(db.employes, "id_employe", "email_employe", transaksi.id_employe);
            return View(transaksi);
        }

        // GET: transaksis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaksi transaksi = db.transaksis.Find(id);
            if (transaksi == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_cart = new SelectList(db.carts, "id_cart", "username_customer", transaksi.id_cart);
            ViewBag.id_employe = new SelectList(db.employes, "id_employe", "email_employe", transaksi.id_employe);
            return View(transaksi);
        }

        // POST: transaksis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_transaksi,QOH,subtotal,id_employe,id_cart,created_at")] transaksi transaksi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaksi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_cart = new SelectList(db.carts, "id_cart", "username_customer", transaksi.id_cart);
            ViewBag.id_employe = new SelectList(db.employes, "id_employe", "email_employe", transaksi.id_employe);
            return View(transaksi);
        }

        // GET: transaksis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            transaksi transaksi = db.transaksis.Find(id);
            if (transaksi == null)
            {
                return HttpNotFound();
            }
            return View(transaksi);
        }

        // POST: transaksis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            transaksi transaksi = db.transaksis.Find(id);
            db.transaksis.Remove(transaksi);
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
    }
}
