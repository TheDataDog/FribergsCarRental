using FribergsCarRental.Data;
using FribergsCarRental.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergsCarRental.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminRepository adminRepository;
        private readonly SessionHelper sessionHelper;

        public AdminController(IAdminRepository adminRepository, SessionHelper sessionHelper)
        {
            this.adminRepository = adminRepository;
            this.sessionHelper = sessionHelper;
        }
        // GET: AdminController
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var admin = adminRepository.GetByEmail(email);
            if(admin == null || admin.Password != password)
            {
                ModelState.AddModelError("", "Ogiltig email eller lösenord"); //funkar ej
                return View("Index");
            }

            SetUserSession(admin.UserRole.Role, admin.AdminId); //Ha kvar som egen metod?

            return View("~/Views/Home/Index.cshtml");
            //return RiderictToAction("Index","Home");
        }

        public void SetUserSession(Role role, int id)
        {
            sessionHelper.SetUserSession(role, id);
        }

        // GET: AdminController/Details/5
        [HttpGet]
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdminController/Delete/5
        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
