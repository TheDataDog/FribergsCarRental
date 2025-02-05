using FribergsCarRental.Data;
using FribergsCarRental.Helpers;
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
            var customerCreateVM = new CustomerCreateViewModel
            {
                Customer = new Customer { UserRole = new UserRole { Role = Role.Customer } }
            };

            var user = GetUserSession();

            if (user.Role == 0)
            {
                ViewBag.User = "Admin";
            }

            return View(customerCreateVM);
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CustomerCreateViewModel customerCreateVM)
        {
            var user = GetUserSession();
            if (user.Role == 0)
            {
                ViewBag.User = "Admin";
            }
            var customers = await customerRepository.GetAllAsync();
            foreach (var customer in customers)
            {
                if (customer.Email == customerCreateVM.Customer.Email)
                {
                    ViewBag.ErrorMsgCreateCustomer = "Det finns redan en registrerad kund med angiven email";
                    if (customerCreateVM.ReturnUrl != null)
                    {
                        ViewBag.Layout = "LoginOrRegister";
                        return View("LoginOrRegister", customerCreateVM);
                    }
                    return View(customerCreateVM);
                }
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    if (customerCreateVM.ReturnUrl != null)
                    {
                        ViewBag.Layout = "LoginOrRegister";
                        return View("LoginOrRegister", customerCreateVM);
                    }
                    return View(customerCreateVM);
                }
                var addedCustomer = await customerRepository.AddAsync(customerCreateVM.Customer);
                if (user.Role == null)
                {
                    SetUserSession(addedCustomer.UserRole.Role, addedCustomer.CustomerId);
                    return RedirectToAction("Create", "Booking");
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.ErrorMsgCreateCustomer = "Något gick fel vid registreringen, försök igen.";
                return View(customerCreateVM);
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
                ViewBag.ErrorMsg = "Något gick fel vid editeringen av kunden, försök igen.";
                return View(customer);
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
                ViewBag.ErrorMsg = "Något gick fel vid borttagning av kunden, försök igen.";
                return View(customer);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var customer = await customerRepository.GetByEmailAsync(model.Email);
            if (customer == null || customer.Password != model.Password)
            {
                ViewBag.ErrorMsg = "Fel email eller lösenord";

                if (model.ReturnUrl != null)
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
