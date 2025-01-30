using FribergsCarRental.Data;
using FribergsCarRental.Models;
using FribergsCarRental.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FribergsCarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository customerRepository;
        private readonly SessionHelper sessionHelper;

        public CustomerController(ICustomerRepository customerRepository, SessionHelper sessionHelper)
        {
            this.customerRepository = customerRepository;
            this.sessionHelper = sessionHelper;
        }
        // GET: CustomerController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(await customerRepository.GetAllAsync());
        }

        // GET: CustomerController/Details/5
        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return View(await customerRepository.GetByIdAsync(id));
        }

        // GET: CustomerController/Create
        [HttpGet]
        public ActionResult Create()
        {
            var customer = new Customer
            {
                UserRole = new UserRole { Role = (Role.Customer) }
            };

            var user = GetUserSession();

            if (user.Role == 0)
            {
                ViewBag.User = "Admin";
            }

            return View(customer);
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Customer customer)
        {
            var user = GetUserSession();

            try
            {
                if (ModelState.IsValid)
                {
                    var addedCustomer = await customerRepository.AddAsync(customer);
                    if (user.Role == null)
                    {
                        SetUserSession(addedCustomer.UserRole.Role, addedCustomer.CustomerId);
                        return RedirectToAction("Create", "Booking");
                    }

                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            return View(await customerRepository.GetByIdAsync(id));
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Customer customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await customerRepository.UpdateAsync(customer);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            return View(await customerRepository.GetByIdAsync(id));
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Customer customer)
        {
            try
            {
                await customerRepository.DeleteAsync(customer);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }
            var customer = await customerRepository.GetByEmailAsync(model.Email);
            if (customer == null || customer.Password != model.Password)
            {
                ViewBag.ErrorMsg = "Fel email eller lösenord";
                
                if(model.ReturnUrl != null)
                {
                    ViewBag.Layout = "LoginOrRegister";
                    return View("LoginOrRegister", model);
                }
                return View(model);
            }
            SetUserSession(customer.UserRole.Role, customer.CustomerId);

            if (sessionHelper.GetCarSession() == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Create", "Booking");
            }

        }

        [HttpGet]
        public ActionResult LoginOrRegister()
        {
            ViewBag.ReturnUrl = "/Customer/LoginOrRegister";
            ViewBag.Layout = "LoginOrRegister";           

            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            sessionHelper.ClearSession();
            return RedirectToAction("Index", "Home");
        }
        public void SetUserSession(Role role, int id)
        {
            sessionHelper.SetUserSession(role, id);
        }

        public (int? Role, int? Id) GetUserSession()
        {
            return sessionHelper.GetUserSession();
        }
    }
}
