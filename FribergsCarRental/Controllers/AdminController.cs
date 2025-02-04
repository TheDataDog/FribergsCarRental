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
    }
}
