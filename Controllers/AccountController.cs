using System.Linq;
using System.Web.Mvc;
using WebClothesMVC.Models; // sesuaikan namespace dengan projek kamu

public class AccountController : Controller
{
    private MyDatabaseCS db = new MyDatabaseCS();

    // GET: /Account/Login
    public ActionResult Login()
    {
        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    public ActionResult Login(string username, string password)
    {
        // Cek di tabel employe
        var employe = db.employes.FirstOrDefault(e => e.username_employe == username && e.password == password);
        if (employe != null)
        {
            Session["username"] = employe.username_employe;
            Session["role"] = employe.akses;

            if (employe.akses == "admin")
                return RedirectToAction("Index", "products");
            else if (employe.akses == "cashier")
                return RedirectToAction("Index", "transaksis");
        }

        // Cek di tabel customer
        var customer = db.customers.FirstOrDefault(c => c.username_customer == username && c.password == password);
        if (customer != null)
        {
            Session["username"] = customer.username_customer;
            Session["role"] = "customer";
            Session["id_customer"] = customer.id_customer;

            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Username atau password salah!";
        return View();
    }

    // GET: /Account/Register
    public ActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    public ActionResult Register(string email, string username, string password)
    {
        if (db.customers.Any(c => c.username_customer == username))
        {
            ViewBag.Error = "Username sudah digunakan.";
            return View();
        }

        var customer = new customer
        {
            email_customer = email,
            username_customer = username,
            password = password
        };

        db.customers.Add(customer);
        db.SaveChanges();

        return RedirectToAction("Login");
    }

    public ActionResult Logout()
    {
        Session.Clear();
        return RedirectToAction("Login");
    }
}
