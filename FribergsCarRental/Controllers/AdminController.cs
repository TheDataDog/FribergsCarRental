using FribergsCarRental.Data;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMsg = "Något gick fel, försök igen.";
                return View("Index", model);
            }

            var admin = await adminRepository.GetByEmailAsync(model.Email);
            if(admin == null || admin.Password != model.Password)
            {
                ViewBag.ErrorMsg = "Fel email eller lösenord";
                return View("Index", model);
            }
            sessionHelper.SetUserSession(admin.UserRole.Role, admin.AdminId);

            return RedirectToAction("Index","Home");
        }

        // GET: AdminController/Details/5
        //[HttpGet]
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AdminController/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AdminController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AdminController/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AdminController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AdminController/Delete/5
        //[HttpGet]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AdminController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
