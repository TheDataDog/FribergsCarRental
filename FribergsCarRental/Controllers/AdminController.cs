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
                ModelState.AddModelError("", "Invalid email or password"); //funkar ej
                return View("Index");
            }

            SetUserSession(admin.UserRole.Role, admin.AdminId); //Ha kvar som egen metod?

            return View("~/Views/Home/Index.cshtml");
            //return RiderictToAction("Index","Home");
        }

        public ActionResult SetUserSession(Role role, int id)
        {
            sessionHelper.SetUserSession(role, id);
            return Content("Session values set via service");
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
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
